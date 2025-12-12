using UnityEngine;

public class ARTouchDamage : MonoBehaviour
{
    [SerializeField] private Camera arCamera;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = arCamera.ScreenPointToRay(touch.position);

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Health targetHealth = hit.collider.GetComponent<Health>();

                    if (targetHealth != null)
                    {
                        float damage = PlayerStats.Instance.CurrentDamage;
                        targetHealth.TakeDamage(damage);

                        Debug.Log("Daño aplicado: " + damage);
                    }
                }
            }
        }
    }
}
