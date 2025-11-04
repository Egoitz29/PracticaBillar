using UnityEngine;

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
    public int Oro = 10;

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
        DontDestroyOnLoad(gameObject);

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

        if (puntosJugador >= puntosRequeridos)
        {
            buttonManager.MostrarPanel1();
            puntosJugador = 0;
            ReiniciarTiros(); // 🟩 Reinicia los tiros al abrir tienda
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
        tirosRestantes = 3;
        Debug.Log("🎯 Tiros reiniciados a 3 (al abrir tienda).");
        ActualizarUI();
    }

    public void SiguienteNivel()
    {
        puntosJugador = 0;
        puntosRequeridos += 2;

        buttonManager.ReiniciarPosiciones();
        jokerManager?.ActualizarVisual();

        uiManager.ActualizarHUD();

        Debug.Log("🚀 Nueva ronda iniciada.");
    }

    public void AddTiros(int cantidad)
    {
        tirosRestantes += cantidad;
        if (tirosRestantes < 0) tirosRestantes = 0;
        Debug.Log($"🎯 Tiros restantes: {tirosRestantes}");
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

        foreach (var bola in bolas)
        {
            if (bola.haTocadoBlanca)
            {
                puntosGanados += bola.rebotes;
            }
        }

        puntosJugador += puntosGanados;
        puntosCalculados = true;

        Debug.Log($"🎯 Turno completado → Rebotes totales: {puntosGanados}, Puntos acumulados: {puntosJugador}, Meta: {puntosRequeridos}");

        uiManager?.ActualizarHUD();

        if (puntosJugador >= puntosRequeridos)
        {
            Debug.Log("🏁 Meta alcanzada o superada → abriendo tienda...");
            buttonManager.MostrarPanel1();
            puntosJugador = 0;
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
