using UnityEngine;
using TMPro;

public class ContadorRebotesAntesDeBlanca : MonoBehaviour
{
    public string tagBolaBlanca = "BolaBlanca";
    public int rebotes { get; set; } = 0;
    public bool haTocadoBlanca { get; private set; } = false;
    public bool haSidoLanzada { get; private set; } = false;

    private BolaFisica bolaFisica;
    private TextMeshPro textoDebug; // 👈 nuevo texto visible en el juego

    void Start()
    {
        bolaFisica = GetComponent<BolaFisica>();
        if (bolaFisica == null)
        {
            Debug.LogError("BolaFisica no encontrada en " + gameObject.name);
            enabled = false;
        }

        // 🧾 Crear texto de depuración en tiempo real
        GameObject textoObj = new GameObject("TextoRebotes_" + gameObject.name);
        textoObj.transform.SetParent(transform);
        textoObj.transform.localPosition = new Vector3(0, 1.2f, 0); // encima de la bola

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

        // 🧠 Actualizar texto constantemente
        if (textoDebug != null)
        {
            textoDebug.text = $"Rebotes: {rebotes}";
        }
    }

    public void ContarRebote()
    {
        if (haSidoLanzada && !haTocadoBlanca)
        {
            int valorRebote = 1;

            // 🃏 Si el jugador tiene el comodín Doble Puntos activo, cada rebote vale 2
            var inventario = FindObjectOfType<InventarioJokerManager>();
            if (inventario != null)
            {
                foreach (var joker in inventario.ObtenerInventario())
                {
                    if (joker != null && joker.tipoEfecto == Joker1.TipoEfecto.DoblePuntos)
                    {
                        valorRebote = 2;
                        break;
                    }
                }
            }

            rebotes += valorRebote;
            Debug.Log($"🟡 {gameObject.name} → Rebote +{valorRebote} (Total: {rebotes})");
        }
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
