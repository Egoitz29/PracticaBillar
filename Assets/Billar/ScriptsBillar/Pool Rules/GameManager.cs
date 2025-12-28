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
    public GameObject panelJuego; // o el nombre que tú use
    public bool dobleReboteActivo = false;

    [Header("Panel de oscurecimiento")]
    public GameObject fondoOscuro; // Asigna un panel negro semitransparente desde la escena


    [Header("Referencia para comodines de escala")]
    public GameObject objetoEscalable; // Asigna en el Inspector el objeto que quieras agrandar



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

        if (tirosRestantes <= 0 && TodasLasBolasQuietas() && puntosJugador < puntosRequeridos)
        {
            Debug.Log(" Sin tiros y sin alcanzar la meta → Guardando resultado y GameOver");

            if (FirebaseDataManager.Instance != null)
            {
                FirebaseDataManager.Instance.SaveGameScore(1, Oro);
                Debug.Log($" Juego 1 guardado → Oro: {Oro}");
            }

            SceneManager.LoadScene("GameOver");
            return;
        }

        if (puntosJugador >= puntosRequeridos)
        {
            // ⬆️ SUBE LA META AL ALCANZAR EL OBJETIVO
            puntosRequeridos += 2;

            //  RESETEA LOS VALORES
            puntosJugador = 0;
            ReiniciarTiros();

            //  REFRESCA EL HUD
            uiManager?.ActualizarHUD();

            //  ABRE LA TIENDA
            buttonManager.MostrarPanel1();

            Debug.Log($" Meta alcanzada → nueva meta: {puntosRequeridos}");
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

        // 🟡 Sumar rebotes de las bolas que tocaron la blanca
        foreach (var bola in bolas)
        {
            if (bola.haTocadoBlanca)
            {
                int valorRebote = bola.rebotes;

                // 💥 Si está activo el comodín de doble rebote, multiplicar
                if (dobleReboteActivo)
                    valorRebote *= 2;

                puntosGanados += valorRebote;
            }
        }

        puntosJugador += puntosGanados;
        puntosCalculados = true;

        uiManager?.ActualizarHUD();

        // 🏁 Si alcanza o supera la meta
        if (puntosJugador >= puntosRequeridos)
        {
            // 💰 Oro ganado = rebotes + tiros restantes
            int oroGanado = Mathf.Max(1, puntosGanados + tirosRestantes);
            Oro += oroGanado;

            Debug.Log($"💰 Meta alcanzada: {puntosGanados} rebotes + {tirosRestantes} tiros restantes → +{oroGanado} oro. Total: {Oro}");

            // 🔁 Reinicio y nueva meta
            puntosRequeridos += 2;
            puntosJugador = 0;
            ReiniciarTiros();

            // 🧠 Aplicar efectos de comodines activos
            var inventario = FindObjectOfType<InventarioJokerManager>();
            if (inventario != null)
            {
                inventario.AplicarEfectosActivosAlInicio();
            }

            uiManager?.ActualizarHUD();

            // 🧩 Desactiva panel de juego (si existe)
            if (panelJuego != null)
            {
                panelJuego.SetActive(false);
                Debug.Log("🧩 Panel de juego desactivado al abrir la tienda.");
            }

            if (buttonManager != null)
            {
                // 🛑 Pausar el juego
                Time.timeScale = 0f;

                // 🩶 Activar fondo oscuro si existe
                if (fondoOscuro != null)
                    fondoOscuro.SetActive(true);

                // 🛒 Mostrar tienda
                buttonManager.MostrarPanel1();
                Debug.Log("🛒 Tienda abierta correctamente (juego pausado).");
            }

            else
            {
                Debug.LogError("❌ ButtonManager no asignado en GameManager. La tienda no se puede abrir.");
            }
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
