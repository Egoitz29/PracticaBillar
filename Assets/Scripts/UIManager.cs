using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections; // ← IMPORTANTE

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoPuntosMeta;
    public TextMeshProUGUI textoPuntosAcumulados;
    public TextMeshProUGUI textoPuntosTurno;
    public TextMeshProUGUI textoTirosRestantes;
    public TextMeshProUGUI textoRondaActual;
    public TextMeshProUGUI textoOro;
    public Button botonOpciones;

    void Start()
    {
        if (botonOpciones) botonOpciones.onClick.AddListener(() => Debug.Log("Menú opciones"));
        ActualizarHUD();
    }

    public void ActualizarHUD()
    {
        var gm = GameManager.Instance;
        if (!gm) return;

        if (textoPuntosMeta) textoPuntosMeta.text = "Meta: " + gm.puntosRequeridos;
        if (textoPuntosAcumulados) textoPuntosAcumulados.text = "Puntos: " + gm.puntosJugador;
        if (textoTirosRestantes) textoTirosRestantes.text = "Tiros: " + gm.tirosRestantes;
        if (textoOro) textoOro.text = "Oro: " + gm.Oro;
    }

    // ✅ Corrutina de parpadeo verde para la "Meta"
    public IEnumerator ParpadeoMetaVerde()
    {
        if (!textoPuntosMeta) yield break;

        Color originalColor = textoPuntosMeta.color;
        Vector3 originalScale = textoPuntosMeta.transform.localScale;
        Color verde = new Color(0.3f, 1f, 0.3f);

        float duracion = 1f;
        float t = 0f;

        while (t < duracion)
        {
            t += Time.deltaTime;
            float osc = Mathf.PingPong(t * 4f, 1f);                 // parpadeo color
            textoPuntosMeta.color = Color.Lerp(originalColor, verde, osc);

            float scaleFactor = 1f + Mathf.Sin(t * 6f) * 0.1f;      // pequeño zoom
            textoPuntosMeta.transform.localScale = originalScale * scaleFactor;

            yield return null;
        }

        textoPuntosMeta.color = originalColor;
        textoPuntosMeta.transform.localScale = originalScale;
    }
}
