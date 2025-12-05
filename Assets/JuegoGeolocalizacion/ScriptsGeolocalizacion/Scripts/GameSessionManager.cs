using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    [Header("Carrera")]
    public int totalZones = 3;
    public int visitedZones = 0;

    [Header("Tiempo")]
    public float startTime;
    public float totalTime;

    [Header("Distancia")]
    public float totalDistance = 0f;
    private Vector3 lastPlayerPos;

    [Header("Puntuación")]
    public int score = 0;

    private Transform player;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        startTime = Time.time;
        lastPlayerPos = player != null ? player.position : Vector3.zero;
        visitedZones = 0;
        score = 0;
        totalDistance = 0f;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player")?.transform;
            if (player != null) lastPlayerPos = player.position;
            return;
        }

        TrackDistance();

        // FIN DE CARRERA: 3 zonas O 30 puntos
        if (visitedZones >= totalZones || score >= 30)
        {
            EndRun();
        }
    }

    void TrackDistance()
    {
        if (player == null) return;
        float d = Vector3.Distance(player.position, lastPlayerPos);
        if (d > 0.1f)
        {
            totalDistance += d;
            lastPlayerPos = player.position;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("Puntos añadidos: " + amount + " ? Total: " + score);
    }

    public void ZoneVisited()
    {
        visitedZones++;
        Debug.Log("Zona visitada: " + visitedZones + "/" + totalZones);
    }

    void EndRun()
    {
        if (SceneManager.GetActiveScene().name == "FinCarrera") return; // Evita bucle

        totalTime = Time.time - startTime;
        Debug.Log($"CARRERA TERMINADA - Tiempo: {totalTime:F1}s | Distancia: {totalDistance:F0}m | Puntos: {score}");
        SceneManager.LoadScene("FinCarrera");
    }
}