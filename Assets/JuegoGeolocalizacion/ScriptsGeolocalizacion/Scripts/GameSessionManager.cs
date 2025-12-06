using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameSessionManager : MonoBehaviour
{
    public static GameSessionManager Instance;

    [Header("Carrera")]
    public int totalZones = 3;
    private int visitedZones = 0;

    // ← NUEVO: Lista para saber qué zonas ya han sido completadas
    private HashSet<int> completedZoneIds = new HashSet<int>();

    [Header("Tiempo")]
    public float startTime;
    public float totalTime;

    [Header("Distancia")]
    public float totalDistance = 0f;
    private Vector3 lastPlayerPos;

    [Header("Puntuación")]
    public int score = 0;

    private Transform player;
    private bool runEnded = false;

    void Awake()
    {
        // Singleton clásico
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

        // Reiniciamos todo al empezar una nueva carrera
        visitedZones = 0;
        completedZoneIds.Clear();
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

        // Cuando ya hemos completado las 3 zonas → Game Over / Fin
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

    /// <summary>
    /// Llamar desde el trigger o script del minijuego cuando el jugador lo completa.
    /// El parámetro zoneId debe ser único por zona (por ejemplo 0, 1 o 2).
    /// </summary>
    public bool TryCompleteZone(int zoneId)
    {
        if (runEnded) return false;

        // Si ya estaba completada → no hacemos nada y devolvemos false
        if (completedZoneIds.Contains(zoneId))
        {
            Debug.LogWarning($"Zona {zoneId} ya fue completada anteriormente.");
            return false;
        }

        // Primera vez que se completa esta zona
        completedZoneIds.Add(zoneId);
        visitedZones++;

        Debug.Log($"Zona {zoneId} completada → {visitedZones}/{totalZones} zonas");

        // Opcional: dar puntos por completar zona
        // AddScore(500);

        return true;
    }

    // Versión antigua por si todavía la usas en algún sitio (la dejamos pero recomendada es TryCompleteZone)
    [System.Obsolete("Usa TryCompleteZone(int zoneId) para evitar repeticiones")]
    public void ZoneVisited()
    {
        if (runEnded) return;
        visitedZones++;
        Debug.LogWarning("ZoneVisited() está obsoleto. Usa TryCompleteZone(zoneId) para evitar que se repita el minijuego.");
    }

    public void EndRun()
    {
        if (runEnded) return;
        runEnded = true;

        totalTime = Time.time - startTime;

        Debug.Log($"=== CARRERA TERMINADA ===");
        Debug.Log($"Zonas completadas: {visitedZones}/{totalZones}");
        Debug.Log($"Puntuación: {score}");
        Debug.Log($"Tiempo: {totalTime:F1}s");
        Debug.Log($"Distancia recorrida: {totalDistance:F1}m");

        // Aquí cambias el nombre de la escena según lo que quieras
        SceneManager.LoadScene("FinCarrera"); // o "GameOver", "Victoria", etc.
    }

    // Útil para debug o UI
    public int GetCompletedZones() => visitedZones;
    public bool IsZoneCompleted(int zoneId) => completedZoneIds.Contains(zoneId);
}