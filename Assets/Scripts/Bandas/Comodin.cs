using UnityEngine;

public abstract class Comodin : MonoBehaviour
{
    // Aplicar el efecto del comodín a la bola
    public abstract void Aplicar(GameObject bola);

    // Para saber qué tipo es visualmente (opcional, útil para debug)
    public abstract Color ObtenerColor();
}
