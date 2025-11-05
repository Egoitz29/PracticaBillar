using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;

public class JokerShopUI : MonoBehaviour
{
    [Header("Referencias UI")]
    public Transform zonaTienda;
    public Transform zonaInventario;
    public TextMeshProUGUI textoOro;
    public GameObject prefabJokerUI;

    [Header("Datos")]
    public List<Joker1> comodinesDisponibles;

    private Vector3 oroEscalaOriginal;
    private Color colorAmarillo = new Color(1f, 0.85f, 0.1f); // amarillo cálido

    private void OnEnable()
    {
        StartCoroutine(EsperarYActualizar());
    }

    private IEnumerator EsperarYActualizar()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);

        ActualizarTienda();
        ActualizarOro();
    }

    public void ActualizarTienda()
    {
        foreach (Transform hijo in zonaTienda)
            Destroy(hijo.gameObject);

        int cantidad = Mathf.Min(2, comodinesDisponibles.Count);
        List<Joker1> copiaLista = new List<Joker1>(comodinesDisponibles);

        for (int i = 0; i < cantidad; i++)
        {
            int randomIndex = Random.Range(0, copiaLista.Count);
            Joker1 j = copiaLista[randomIndex];
            copiaLista.RemoveAt(randomIndex);

            CrearCartaJoker(j, zonaTienda);
        }
    }

    void CrearCartaJoker(Joker1 joker, Transform parent)
    {
        GameObject carta = Instantiate(prefabJokerUI, parent);

        var nombreText = carta.transform.Find("Nombre")?.GetComponent<TextMeshProUGUI>();
        var precioText = carta.transform.Find("Precio")?.GetComponent<TextMeshProUGUI>();
        var iconoImg = carta.transform.Find("Icono")?.GetComponent<Image>();
        var descText = carta.transform.Find("Descripcion")?.GetComponent<TextMeshProUGUI>();

        if (!nombreText || !precioText || !iconoImg || !descText)
        {
            Debug.LogError("❌ JokerUI no tiene hijos: Icono/Nombre/Precio/Descripcion correctamente nombrados.");
            return;
        }

        nombreText.text = joker.nombre;
        precioText.text = joker.precioCompra + "💰";
        iconoImg.sprite = joker.icono;
        descText.text = joker.descripcion;

        var drag = carta.GetComponent<DraggableJoker>();
        if (drag == null) drag = carta.AddComponent<DraggableJoker>();
        drag.jokerData = joker;
    }

    // ============================================================
    // 💰 ACTUALIZAR ORO + ANIMACIÓN VISUAL Y COLOR DINÁMICO
    // ============================================================
    public void ActualizarOro(int cambio = 0)
    {
        if (textoOro == null)
        {
            Debug.LogWarning("⚠️ No se ha asignado el campo 'textoOro' en JokerShopUI.");
            return;
        }

        textoOro.text = "Oro: " + GameManager.Instance.Oro.ToString();

        if (oroEscalaOriginal == Vector3.zero)
            oroEscalaOriginal = textoOro.transform.localScale;

        StopAllCoroutines();
        StartCoroutine(ReboteYColor(cambio));
    }

    private IEnumerator ReboteYColor(int cambio)
    {
        float duracion = 0.15f;
        float tiempo = 0f;
        Vector3 grande = oroEscalaOriginal * 1.2f;

        // Determina color según ganancia o gasto
        Color colorObjetivo = cambio > 0 ? Color.green : cambio < 0 ? Color.red : colorAmarillo;

        // Transición de color
        textoOro.color = colorObjetivo;

        // Animación de escala (pop)
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            textoOro.transform.localScale = Vector3.Lerp(oroEscalaOriginal, grande, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // Escala de vuelta
        tiempo = 0f;
        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;
            textoOro.transform.localScale = Vector3.Lerp(grande, oroEscalaOriginal, Mathf.SmoothStep(0, 1, t));
            yield return null;
        }

        // Volver al color amarillo suave
        float fadeTime = 0.3f;
        Color actual = textoOro.color;
        float tColor = 0f;
        while (tColor < fadeTime)
        {
            tColor += Time.deltaTime;
            textoOro.color = Color.Lerp(actual, colorAmarillo, tColor / fadeTime);
            yield return null;
        }

        textoOro.color = colorAmarillo;
        textoOro.transform.localScale = oroEscalaOriginal;
    }
}
