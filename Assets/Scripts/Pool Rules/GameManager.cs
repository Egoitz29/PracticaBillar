using UnityEngine;

// GameManager controla el estado general del juego, incluyendo puntos, tiros, niveles y lógica de fin de turno
public class GameManager : MonoBehaviour
{
    // Patrón Singleton para acceder al GameManager desde cualquier otro script
    public static GameManager Instance { get; private set; }

    [Header("Valores actuales")]
    public int tirosRestantes = 3;        // Tiros que le quedan al jugador en este turno/ronda
    public int puntosJugador = 0;         // Puntos acumulados por el jugador
    public int puntosRequeridos = 3;      // Puntos necesarios para ganar esta ronda

    [Header("Valores iniciales")]
    public int tirosRestantesInicio = 3;  // Tiros iniciales al comenzar una partida/ronda
    public int puntosJugadorInicio = 0;   // Puntos iniciales (usualmente 0)
    public int puntosRequeridosInicio = 3;// Puntos necesarios al inicio (aumentan con cada nivel)

    public ButtonManager buttonManager;   // Referencia al sistema de UI que gestiona los botones y paneles
    private JokerManager jokerManager;    // Referencia al sistema de jokers (bonus especiales)

    private ContadorRebotesAntesDeBlanca[] bolas; // Todas las bolas en escena que vamos a evaluar
    private bool puntosCalculados = true;        // Evita calcular los puntos varias veces por turno

    public UIManager uiManager;
    public int Oro = 10;

    // Se ejecuta antes del Start
    private void Awake()
    {
        // Busca todas las bolas con componente ContadorRebotesAntesDeBlanca en escena
        bolas = FindObjectsOfType<ContadorRebotesAntesDeBlanca>();

        // Configura el Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Si ya hay otro GameManager, elimina este
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Hace que el GameManager persista entre escenas si usas más de una

        // Asigna referencias a componentes que están en el mismo objeto
        buttonManager = GetComponent<ButtonManager>();
        jokerManager = GetComponent<JokerManager>();

        ReiniciarEstado(); // Inicializa los valores del juego

        ActualizarUI();
    }

    // Se ejecuta cada frame
    private void Update()
    {
        // Si todas las bolas están quietas y aún no se han calculado los puntos, calcula puntos
        if (!puntosCalculados && TodasLasBolasQuietas())
        {
            CalcularPuntosTurno();
            puntosCalculados = true;
        }

        // Si ya se han calculado los puntos y todas las bolas siguen quietas, pasa al siguiente turno
        if (puntosCalculados && TodasLasBolasQuietas())
        {
            EmpezarNuevoTurno();
        }

        // Si el jugador se queda sin tiros, reinicia la ronda (NO la escena)
        if (tirosRestantes < 0)
        {
            ReiniciarRonda();
        }

        // Si el jugador consigue los puntos necesarios, muestra panel de victoria y reinicia puntaje
        if (puntosJugador >= puntosRequeridos)
        {
            buttonManager.MostrarPanel1(); // Por ejemplo, panel de victoria
            Debug.Log("¡Has ganado esta ronda! Puntos totales: " + puntosJugador);
            puntosJugador = 0; // Se puede reiniciar aquí o más tarde según el flujo
        }

        ActualizarUI();
    }

    // Reinicia los valores base al comenzar una nueva partida completa
    public void ReiniciarEstado()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;
        puntosRequeridos = puntosRequeridosInicio;
    }

    // Reinicia la ronda actual sin cambiar el nivel
    public void ReiniciarRonda()
    {
        puntosJugador = puntosJugadorInicio;
        tirosRestantes = tirosRestantesInicio;

        buttonManager.ReiniciarPosiciones(); // Reubica bolas, reinicia físicas, etc.
    }

    // Avanza al siguiente nivel
    public void SiguienteNivel()
    {
        puntosJugador = 0;
        tirosRestantes = tirosRestantesInicio;
        puntosRequeridos += 2; // Hace el juego más difícil cada nivel

        buttonManager.ReiniciarPosiciones(); // Vuelve a colocar las bolas

        AplicarJokersInicioTurno(); // Aplica jokers que tengan efecto al empezar turno
    }

    // Aumenta la cantidad de tiros disponibles (ej: por un joker)
    public void AddTiros(int cantidad)
    {
        tirosRestantes++;
        Debug.Log("Tiro sumado. Total: ");
    }

    // Multiplica el puntaje actual por un factor (ej: x2 por un joker)
    public void MultiplicarPuntaje(float factor)
    {
        puntosJugador = Mathf.RoundToInt(puntosJugador * factor); // Redondea al entero más cercano
    }

    // Aplica jokers que se activan al comienzo del turno
    public void AplicarJokersInicioTurno()
    {
        jokerManager.ActivarJokersInicio(this);
    }

    // También aplica jokers, pero este método podría usarse para jokers de final de turno
    public void AplicarJokersFinalTurno()
    {
        jokerManager.ActivarJokersFinal(this); // Aquí podrías llamar a otro método más adelante
    }

    // Devuelve true si todas las bolas están quietas (no se están moviendo)
    bool TodasLasBolasQuietas()
    {
        foreach (var bola in bolas)
        {
            if (!bola.EstaQuieto())
                return false;
        }
        return true;
    }

    // Suma los puntos por cada bola que haya tocado la blanca, basado en los rebotes
    void CalcularPuntosTurno()
    {
        foreach (var bola in bolas)
        {
            if (bola.haTocadoBlanca)
            {       
                puntosJugador += bola.rebotes*1;
            }
            else
            {
                puntosJugador += bola.rebotes * 0;
            }
           
        }
        AplicarJokersFinalTurno(); // Aplica jokers que tengan efecto al final del turno

       
    }

    // Llama al reset de cada bola y prepara para un nuevo turno
    public void EmpezarNuevoTurno()
    {
        foreach (var bola in bolas)
            bola.Resetear();

        puntosCalculados = false; // Permite calcular puntos de nuevo cuando todas las bolas se detengan
        //Debug.Log("Puntos totales del turno: " + puntosJugador);
    }

    public ContadorRebotesAntesDeBlanca[] ObtenerTodasLasBolas()
    {
        return bolas;
    }

    void ActualizarUI()
    {
        uiManager.puntosMeta = puntosRequeridos;
        uiManager.puntosAcumulados = puntosJugador;
        //uiManager.puntosTurno = puntosTurno; falta
        uiManager.tirosRestantes = tirosRestantes;
        //uiManager.rondaActual = rondaActual; falta
        //uiManager.oro = oro; falta

        uiManager.ActualizarHUD();
    }

}
