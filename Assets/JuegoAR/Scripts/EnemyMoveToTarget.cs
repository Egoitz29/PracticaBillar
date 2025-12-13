using UnityEngine;

public class EnemyMoveToTarget : MonoBehaviour
{
    public float speed = 1.5f;

    private Transform target;
    private TargetZone targetZone;

    void Update()
    {
        if (target == null || targetZone == null) return;

        // Mantener la altura de la torre
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        float distance = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, transform.position.z), targetPos);

        if (distance <= targetZone.radius)
            return;

        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        targetZone = newTarget.GetComponent<TargetZone>();
    }
}
