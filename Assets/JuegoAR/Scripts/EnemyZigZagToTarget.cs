using UnityEngine;

public class EnemyZigZagToTarget : MonoBehaviour
{
    [Header("Movimiento ZigZag")]
    public float speed = 2f;
    public float zigzagSpeed = 5f;
    public float zigzagAmount = 0.5f;

    private Transform target;
    private float zigzagTime;

    void Update()
    {
        if (target == null) return;

        zigzagTime += Time.deltaTime * zigzagSpeed;

        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 zigzagOffset = transform.right * Mathf.Sin(zigzagTime) * zigzagAmount;

        Vector3 finalDirection = (direction + zigzagOffset).normalized;

        transform.position += finalDirection * speed * Time.deltaTime;
    }

    // 🔗 El spawner asigna la torre
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
