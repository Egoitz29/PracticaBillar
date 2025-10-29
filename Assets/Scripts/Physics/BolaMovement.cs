using UnityEngine;

public class BolaFisica : MonoBehaviour
{
    public float radio = 0.15f;
    public Vector2 velocidad = Vector2.zero;

    [Range(0f, 1f)]
    public float friccion = 0.98f;

    public float velocidadMinima = 0.05f;

    public float minX = -4.5f;
    public float maxX = 4.5f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

    public bool EstaEnMovimiento => velocidad.magnitude > velocidadMinima;

    void FixedUpdate()
    {
        if (velocidad.magnitude > velocidadMinima)
        {
            transform.position += (Vector3)(velocidad * Time.fixedDeltaTime);
            velocidad *= friccion;
        }
        else
        {
            velocidad = Vector2.zero;
        }


    }

    public void AplicarVelocidad(Vector2 nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public Vector2 ObtenerVelocidad()
    {
        return velocidad;
    }
}


