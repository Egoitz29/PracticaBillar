using UnityEngine;

// Componente que mueve la esfera hacia un objetivo
public class MoveToTarget : MonoBehaviour
{
    private Vector3 target;
    private float speed;

    public void Initialize(Vector3 targetPoint, float moveSpeed)
    {
        target = targetPoint;
        speed = moveSpeed;
    }

    void Update()
    {
        // Mover hacia el target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // Destruir al llegar
        if (Vector3.Distance(transform.position, target) < 0.01f)
        {
            Destroy(gameObject);
        }
    }
}
