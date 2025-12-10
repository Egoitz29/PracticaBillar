using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTouchDamage : MonoBehaviour
{
    [SerializeField] private Camera arCamera; // La cámara de AR
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private GameObject hitEffectPrefab; // Prefab de la esfera/effect visual
    [SerializeField] private float effectSpeed = 5f; // Velocidad a la que se mueve la esfera
    [SerializeField] private Transform spawnPoint; // Punto desde donde se instanciará la esfera

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
                    // Aplicar daño si existe componente Health
                    Health targetHealth = hit.collider.GetComponent<Health>();
                    if (targetHealth != null)
                    {
                        targetHealth.TakeDamage(damageAmount);
                        Debug.Log("Daño aplicado a: " + hit.collider.name);
                    }

                    // Instanciar efecto visual desde spawnPoint
                    if (hitEffectPrefab != null && spawnPoint != null)
                    {
                        GameObject effect = Instantiate(hitEffectPrefab, spawnPoint.position, Quaternion.identity);
                        // Hacer que se mueva hacia el punto de hit
                        effect.AddComponent<MoveToTarget>().Initialize(hit.point, effectSpeed);
                    }
                }
            }
        }
    }
}
