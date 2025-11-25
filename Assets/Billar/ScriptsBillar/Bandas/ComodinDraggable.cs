using UnityEngine;

public class ComodinDraggable : MonoBehaviour
{
    private Vector3 offset;
    private bool arrastrando = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        offset = transform.position - cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        arrastrando = true;
    }

    private void OnMouseUp()
    {
        arrastrando = false;

        // Raycast para detectar si estamos soltando sobre una banda
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Banda banda = hit.collider.GetComponent<Banda>();
            if (banda != null)
            {
                if (banda.AñadirComodin(this.GetComponent<Comodin>()))
                {
                    Destroy(this); // Destruye el script draggable después de colocarse
                }
                else
                {
                    Debug.Log("La banda ya tiene 3 comodines");
                    ResetPosicion();
                }
            }
            else
            {
                ResetPosicion();
            }
        }
        else
        {
            ResetPosicion();
        }
    }

    private void OnMouseDrag()
    {
        if (arrastrando)
        {
            Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
            transform.position = mousePos + offset;
        }
    }

    private void ResetPosicion()
    {
        // Puedes reubicar el comodín en su zona original si quieres
    }
}
