using UnityEngine;

public class TargetZone : MonoBehaviour
{
    public float radius = 1f;
    public TowerHealth towerHealth;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}