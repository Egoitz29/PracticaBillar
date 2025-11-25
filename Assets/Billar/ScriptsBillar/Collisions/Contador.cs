using UnityEngine;
using TMPro;
using System.Collections;

public class ContadorRebotesAntesDeBlanca : MonoBehaviour
{
    public string tagBolaBlanca = "BolaBlanca";
    public int rebotes { get; set; } = 0;
    public bool haTocadoBlanca { get; private set; } = false;
    public bool haSidoLanzada { get; private set; } = false;

    private BolaFisica bolaFisica;
    private TextMeshPro textoDebug;
    private Coroutine efectoX2;

    void Start()
    {
        bolaFisica = GetComponent<BolaFisica>();
        if (bolaFisica == null)
        {
            Debug.LogError("❌ BolaFisica no encontrada en " + gameObject.name);
            enabled = false;
            return;
        }

        GameObject textoObj = new GameObject("TextoRebotes_" + gameObject.name);
        textoObj.transform.SetParent(transform);
        textoObj.transform.localPosition = new Vector3(0, 1.2f, 0);

        textoDebug = textoObj.AddComponent<TextMeshPro>();
        textoDebug.fontSize = 3f;
        textoDebug.alignment = TextAlignmentOptions.Center;
        textoDebug.color = Color.yellow;
        textoDebug.text = "Rebotes: 0";
    }

    void Update()
    {
        if (bolaFisica.EstaEnMovimiento && !haSidoLanzada)
            haSidoLanzada = true;

        if (textoDebug != null)
        {
            bool x2Activo = GameManager.Instance != null && GameManager.Instance.dobleReboteActivo;
            textoDebug.text = x2Activo ? $"Rebotes: {rebotes}  x2" : $"Rebotes: {rebotes}";
        }
    }

    public void ContarRebote()
    {
        if (haSidoLanzada && !haTocadoBlanca)
        {
            int valorRebote = 1;

            if (GameManager.Instance != null && GameManager.Instance.dobleReboteActivo)
            {
                valorRebote = 2;

                if (efectoX2 == null)
                    efectoX2 = StartCoroutine(EfectoParpadeoX2());
            }

            rebotes += valorRebote;
            Debug.Log($"🟡 {gameObject.name} → Rebote +{valorRebote} (Total: {rebotes})");
        }
    }

    private IEnumerator EfectoParpadeoX2()
    {
        float tiempo = 0f;
        while (GameManager.Instance != null && GameManager.Instance.dobleReboteActivo)
        {
            tiempo += Time.deltaTime * 3f;
            textoDebug.color = Color.Lerp(Color.yellow, new Color(1f, 0.8f, 0f), Mathf.PingPong(tiempo, 1f));
            textoDebug.alpha = Mathf.PingPong(tiempo * 0.5f, 0.5f) + 0.5f;
            yield return null;
        }

        textoDebug.color = Color.yellow;
        textoDebug.alpha = 1f;
        efectoX2 = null;
    }

    public void VerificarBolaBlanca(GameObject otraBola)
    {
        if (!haTocadoBlanca && otraBola.CompareTag(tagBolaBlanca))
            haTocadoBlanca = true;
    }

    public bool EstaQuieto() => !bolaFisica.EstaEnMovimiento;

    public void Resetear()
    {
        rebotes = 0;
        haTocadoBlanca = false;
        haSidoLanzada = false;
    }
}
