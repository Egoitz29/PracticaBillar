using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    public Transform target;   // El objeto al que debe ir
    public float speed = 1.5f;
    public float destroyDistance = 0.2f;

    void Update()
    {
        if (target == null) return;

        // Mover hacia el objetivo
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );

        // Si llega al objetivo -> se destruye
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance <= destroyDistance)
        {
            Debug.Log("El enemigo llegó al objetivo y se destruye.");
            Destroy(gameObject);
        }
    }
}
