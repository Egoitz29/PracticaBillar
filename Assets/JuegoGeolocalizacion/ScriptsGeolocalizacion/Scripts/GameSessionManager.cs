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
    private bool runEnded = false; //  Para evitar múltiples llamadas

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
        runEnded = false;
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

        if (visitedZones >= totalZones && !runEnded)
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
        Debug.Log("Puntos: " + score);
    }

    public void ZoneVisited()
    {
        if (runEnded) return;

        visitedZones++;
        Debug.Log($"Zona visitada: {visitedZones}/{totalZones}");

        // if (visitedZones >= totalZones) EndRun();
    }

    public void EndRun()
    {
        if (runEnded) return;
        runEnded = true;

        totalTime = Time.time - startTime;

        Debug.Log($"CARRERA TERMINADA - ZONAS: {visitedZones}/3 | Puntos: {score} | Tiempo: {totalTime:F1}s");

        SceneManager.LoadScene("FinCarrera"); 
    }
}