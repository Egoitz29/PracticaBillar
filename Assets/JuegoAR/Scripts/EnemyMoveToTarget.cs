using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 1.5f;

    private Transform target;

    void Update()
    {
        if (target == null) return;

        // Mover hacia el objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    // 🔗 El spawner llamará a esto
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
