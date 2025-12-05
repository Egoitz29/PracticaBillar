using UnityEngine;
using System.Collections.Generic;

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
        player = GameObject.FindWithTag("Player").transform;

        startTime = Time.time;
        lastPlayerPos = player.position;

        totalZones = 3;
        visitedZones = 0;
        score = 0;
        totalDistance = 0f;
    }

    void Update()
    {
        if (player == null) return;

        TrackDistance();
        ZoneVisited();

        if (score >= 30)
        {
            EndRun();
        }
    }

    void TrackDistance()
    {
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
    }

    public void ZoneVisited()
    {
        visitedZones++;

        if (visitedZones >= totalZones)
            EndRun();
    }

    void EndRun()
    {
        totalTime = Time.time - startTime;

        Debug.Log("CARRERA COMPLETADA");
        Debug.Log("Tiempo total: " + totalTime);
        Debug.Log("Distancia total: " + totalDistance);
        Debug.Log("Puntuación final: " + score);

        UnityEngine.SceneManagement.SceneManager.LoadScene("FinCarrera");
    }
}
