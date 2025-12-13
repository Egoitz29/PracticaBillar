using UnityEngine;

public class CircularEnemySpawner : MonoBehaviour
{
    [Header("Tower (centro)")]
    public Transform tower;
    public TowerHealth towerHealth;

    [Header("Tipos de enemigos")]
    public GameObject enemyNormal;
    public GameObject enemyFast;
    public GameObject enemyZigZag;

    [Header("Spawn circular")]
    public float spawnRadius = 10f;

    [Header("Dificultad")]
    public float initialInterval = 4f;
    public float minInterval = 1f;
    public float difficultyStep = 0.1f;

    private float currentInterval;
    private bool isActive = false;

    void Start()
    {
        currentInterval = initialInterval;
    }

    // 🔹 Controlado por RoundManager
    public void SetActive(bool value)
    {
        isActive = value;

        CancelInvoke();

        if (isActive)
        {
            Invoke(nameof(SpawnLoop), currentInterval);
        }
    }

    void SpawnLoop()
    {
        if (!isActive) return;

        SpawnEnemy();

        currentInterval -= difficultyStep;
        if (currentInterval < minInterval)
            currentInterval = minInterval;

        Invoke(nameof(SpawnLoop), currentInterval);
    }

    void SpawnEnemy()
    {
        if (tower == null || towerHealth == null) return;

        float angle = Random.Range(0f, 360f);
        float rad = angle * Mathf.Deg2Rad;

        Vector3 spawnPos = new Vector3(
            tower.position.x + Mathf.Cos(rad) * spawnRadius,
            tower.position.y,
            tower.position.z + Mathf.Sin(rad) * spawnRadius
        );

        GameObject prefab = ChooseEnemyByDifficulty();
        GameObject enemy = Instantiate(prefab, spawnPos, Quaternion.identity);

        // 🔗 Asignar TARGET a cualquier tipo de movimiento
        EnemyMoveToTarget moveNormal = enemy.GetComponent<EnemyMoveToTarget>();
        if (moveNormal != null)
            moveNormal.SetTarget(tower);

        EnemyZigZagToTarget moveZigZag = enemy.GetComponent<EnemyZigZagToTarget>();
        if (moveZigZag != null)
            moveZigZag.SetTarget(tower);

        // 🔗 Asignar TORRE para el daño
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
