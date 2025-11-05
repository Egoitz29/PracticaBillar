using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Valores actuales")]
    public int tirosRestantes = 3;
    public int puntosJugador = 0;
    public int puntosRequeridos = 3;

    [Header("Valores iniciales")]
    public int tirosRestantesInicio = 3;
    public int puntosJugadorInicio = 0;
    public int puntosRequeridosInicio = 3;

    public ButtonManager buttonManager;
    public UIManager uiManager;
    public int Oro = 0;

    private JokerManager jokerManager;
    private ContadorRebotesAntesDeBlanca[] bolas;
    private bool puntosCalculados = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        bolas = FindObjectsOfType<ContadorRebotesAntesDeBlanca>();
        buttonManager = GetComponent<ButtonManager>();
        jokerManager = GetComponent<JokerManager>();

        ReiniciarEstado();
        ActualizarUI();


    }

    private void Update()
    {
        if (!puntosCalculados && TodasLasBolasQuietas())
        {
            CalcularPuntosTurno();
            puntosCalculados = true;
        }

        if (puntosCalculados && TodasLasBolasQuietas())
        {
            EmpezarNuevoTurno();
        }

        if (tirosRestantes < 0)
        {
            ReiniciarRonda();
        }

        //  Nueva condición: sin tiros, bolas quietas y sin haber llegado a la meta → GameOver
        if (tirosRestantes <= 0 && TodasLasBolasQuietas() && puntosJugador < puntosRequeridos)
        {
            Debug.Log(" Sin tiros y sin alcanzar la meta → Cargando escena GameOver...");
            SceneManager.LoadScene("GameOver");
            return;
        }

        if (puntosJugador >= puntosRequeridos)
        {
            // ⬆️ SUBE LA META AL ALCANZAR EL OBJETIVO
            puntosRequeridos += 2;

            // 🔄 RESETEA LOS VALORES
            puntosJugador = 0;
            ReiniciarTiros();

            // 🧠 REFRESCA EL HUD
            uiManager?.ActualizarHUD();

            // 🛒 ABRE LA TIENDA
            buttonManager.MostrarPanel1();

            Debug.Log($"🎯 Meta alcanzada → nueva meta: {puntosRequeridos}");
        }



        ActualizarUI();
    }

    public void ReiniciarEstado()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;
        puntosRequeridos = puntosRequeridosInicio;
    }

    public void ReiniciarRonda()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;
        buttonManager.ReiniciarPosiciones();
    }

    public void ReiniciarTiros()
    {
        // 🎯 Reinicia los tiros base
        tirosRestantes = 3;
        Debug.Log("🎯 Tiros reiniciados a 3 (inicio de ronda o al abrir tienda).");

        // 🧠 Si hay comodín de +1 Tiro, se aplica automáticamente
        var inventario = FindObjectOfType<InventarioJokerManager>();
        if (inventario != null)
        {
            foreach (var joker in inventario.ObtenerInventario())
            {
                if (joker.tipoEfecto == Joker1.TipoEfecto.MasTiro)
                {
                    tirosRestantes += 1;
                    Debug.Log($"✨ [{joker.nombre}] activo → +1 tiro extra (total: {tirosRestantes})");
                }
            }
        }

        ActualizarUI();
    }

    public void SiguienteNivel()
    {
        puntosJugador = 0;

        buttonManager.ReiniciarPosiciones();
        jokerManager?.ActualizarVisual();

        uiManager.ActualizarHUD();

        Debug.Log(" Nueva ronda iniciada.");
    }


    public void AddTiros(int cantidad)
    {
        tirosRestantes += cantidad;
        if (tirosRestantes < 0) tirosRestantes = 0;
        Debug.Log($" Tiros restantes: {tirosRestantes}");
        ActualizarUI();
    }

    bool TodasLasBolasQuietas()
    {
        foreach (var bola in bolas)
        {
            if (!bola.EstaQuieto())
                return false;
        }
        return true;
    }
    void CalcularPuntosTurno()
    {
        int puntosGanados = 0;

        // 🟡 Sumamos todos los rebotes de las bolas que tocaron la blanca
        foreach (var bola in bolas)
        {
            if (bola.haTocadoBlanca)
            {
                puntosGanados += bola.rebotes;
            }
        }

        puntosJugador += puntosGanados;
        puntosCalculados = true;

        uiManager?.ActualizarHUD();

        // 🏁 Si alcanza o supera la meta
        if (puntosJugador >= puntosRequeridos)
        {
            // 💰 Calculamos oro ganado = rebotes + tiros restantes
            int oroGanado = Mathf.Max(1, puntosGanados + tirosRestantes);

            // 🔹 Sumamos al total
            Oro += oroGanado;

            // 🧠 Log bonito
            Debug.Log($"💰 Meta alcanzada: {puntosGanados} rebotes + {tirosRestantes} tiros restantes → +{oroGanado} oro. Total: {Oro}");

            // 🔹 Subimos la meta base +2 (como antes)
            puntosRequeridos += 2;

            // 🔄 Reiniciamos jugador y tiros (base)
            puntosJugador = 0;
            ReiniciarTiros();

            // 🧠 Aplica efectos activos del inventario (como +1 Tiro o -1 Meta)
            FindObjectOfType<InventarioJokerManager>()?.AplicarEfectosActivosAlInicio();

            // 🧾 Actualizamos interfaz
            uiManager?.ActualizarHUD();

            // 🛒 Abrimos la tienda
            buttonManager.MostrarPanel1();
        }
    }



    public void EmpezarNuevoTurno()
    {
        foreach (var bola in bolas)
            bola.Resetear();

        puntosCalculados = false;
    }

    void ActualizarUI()
    {
        if (uiManager != null)
            uiManager.ActualizarHUD();
    }


}
