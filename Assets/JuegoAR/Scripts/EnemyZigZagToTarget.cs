using UnityEngine;

public class EnemyZigZagToTarget : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float zigzagSpeed = 5f;
    public float zigzagAmount = 0.5f;
    public float destroyDistance = 0.2f;

    private float zigzagTime;

    void Update()
    {
        if (target == null) return;

        zigzagTime += Time.deltaTime * zigzagSpeed;

        Vector3 direction = (target.position - transform.position).normalized;

        Vector3 zigzagOffset = transform.right * Mathf.Sin(zigzagTime) * zigzagAmount;

        Vector3 finalDirection = direction + zigzagOffset;

        transform.position += finalDirection.normalized * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.position) <= destroyDistance)
        {
            Destroy(gameObject);
        }
    }
}
