using UnityEngine;

public class CPuntosExtra : Comodin
{
    public int puntosExtra = 1;

    public override void Aplicar(GameObject bola)
    {
        // Aquí deberías tener una referencia a algún sistema de puntuación
        Debug.Log("Se aplican " + puntosExtra + " puntos extra");
        // Ejemplo: GameManager.Instance.AñadirPuntos(puntosExtra);
    }

    public override Color ObtenerColor()
    {
        return Color.yellow;
    }
}
