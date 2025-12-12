using UnityEngine;

public class EnemyDamageTower : MonoBehaviour
{
    [Header("Daño a la torre")]
    public float damageToTower = 10f;
    public float reachDistance = 1.2f;

    private TowerHealth tower;

    void Update()
    {
        if (tower == null) return;

        float distance = Vector3.Distance(
            transform.position,
            tower.transform.position
        );

        if (distance <= reachDistance)
        {
            tower.TakeDamage(damageToTower);
            Destroy(gameObject);
        }
    }

    // 🔗 El spawner asigna la torre
    public void SetTower(TowerHealth newTower)
    {
        tower = newTower;
    }
}
