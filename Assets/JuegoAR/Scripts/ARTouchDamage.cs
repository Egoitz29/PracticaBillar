using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTouchDamage : MonoBehaviour
{
    [SerializeField] private Camera arCamera; // La cámara de AR
    [SerializeField] private float damageAmount = 10f;

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
                    // Intentamos obtener un componente de vida
                    Health targetHealth = hit.collider.GetComponent<Health>();
                    if (targetHealth != null)
                    {
                        targetHealth.TakeDamage(damageAmount);
                        Debug.Log("Daño aplicado a: " + hit.collider.name);
                    }
                }
            }
        }
    }
}
