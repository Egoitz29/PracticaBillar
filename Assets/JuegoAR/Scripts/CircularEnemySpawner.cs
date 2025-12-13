using UnityEngine;

public class CircularEnemySpawner : MonoBehaviour
{
    public static CircularEnemySpawner Instance;

    [Header("Torre (TargetZone)")]
    public TargetZone targetZone;
    public TowerHealth towerHealth;

    [Header("Tipos de enemigos")]
    public GameObject enemyNormal;
    public GameObject enemyFast;
    public GameObject enemyZigZag;

    [Header("Spawn circular")]
    public float spawnRadius = 1.5f;

    [Header("Dificultad")]
    public float initialInterval = 4f;
    public float minInterval = 1f;
    public float difficultyStep = 0.1f;

    private float currentInterval;
    private bool isActive = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        currentInterval = initialInterval;
        isActive = false; // No activo al inicio
    }

    public void SetActive(bool value)
    {
        isActive = value;
        CancelInvoke();

        if (isActive && targetZone != null)
            Invoke(nameof(SpawnLoop), currentInterval);
    }

    void SpawnLoop()
    {
        if (!isActive || targetZone == null) return;

        SpawnEnemy();

        currentInterval -= difficultyStep;
        if (currentInterval < minInterval)
            currentInterval = minInterval;

        Invoke(nameof(SpawnLoop), currentInterval);
    }

    void SpawnEnemy()
    {
        if (targetZone == null || towerHealth == null) return;

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 center = targetZone.transform.position;

        Vector3 spawnPos = new Vector3(
            center.x + Mathf.Cos(rad) * spawnRadius,
            center.y,
            center.z + Mathf.Sin(rad) * spawnRadius
        );

        GameObject prefab = ChooseEnemyByDifficulty();
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        // Asignar TargetZone como objetivo
        EnemyMoveToTarget moveNormal = enemy.GetComponent<EnemyMoveToTarget>();
        if (moveNormal != null)
            moveNormal.SetTarget(targetZone.transform);

        EnemyZigZagToTarget moveZigZag = enemy.GetComponent<EnemyZigZagToTarget>();
        if (moveZigZag != null)
            moveZigZag.SetTarget(targetZone.transform);

        // Asignar daño a la torre
        EnemyDamageTower damageTower = enemy.GetComponent<EnemyDamageTower>();
        if (damageTower != null)
            damageTower.SetTower(towerHealth);
    }

    GameObject ChooseEnemyByDifficulty()
    {
        float time = Time.time;

        if (time < 20f)
            return enemyNormal;
        if (time < 40f)
            return Random.value < 0.7f ? enemyNormal : enemyFast;

        float r = Random.value;
        if (r < 0.5f) return enemyNormal;
        if (r < 0.8f) return enemyFast;
        return enemyZigZag;
    }
}
