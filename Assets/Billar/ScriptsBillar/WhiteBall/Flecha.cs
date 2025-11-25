using UnityEngine;

public class FlechaVisual : MonoBehaviour
{
    public Transform flecha; // Sprite o barra como hijo de la bola
    public float fuerzaMaxima = 10f;     // Distancia máxima que consideraremos como "máxima fuerza"
    public float escalaMaxima = 2f;      // Longitud máxima visual de la flecha

    private bool arrastrando = false;
    private Vector2 puntoInicio;

    void Start()
    {
        flecha.gameObject.SetActive(false);
    }

    void Update()
    {
        Vector2 mouseMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Iniciar arrastre
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector2.Distance(mouseMundo, transform.position) < 0.5f)
            {
                arrastrando = true;
                puntoInicio = transform.position;
                flecha.localPosition = Vector3.zero;
                flecha.gameObject.SetActive(true);
            }
        }

        // Mostrar y actualizar flecha
        if (Input.GetMouseButton(0) && arrastrando)
        {
            Vector2 direccion = (puntoInicio - mouseMundo);
            float distancia = Mathf.Clamp(direccion.magnitude, 0, fuerzaMaxima);

            // Rotar flecha
            float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
            flecha.rotation = Quaternion.Euler(0, 0, angulo);

            // Escalar flecha en X (longitud)
            float escala = Mathf.Lerp(0.1f, escalaMaxima, distancia / fuerzaMaxima);
            flecha.localScale = new Vector3(escala, flecha.localScale.y, flecha.localScale.z);
        }

        // Finalizar arrastre
        if (Input.GetMouseButtonUp(0))
        {
            arrastrando = false;
            flecha.gameObject.SetActive(false);
        }
    }
}
