using UnityEngine;

public class CMultiplicador : Comodin
{
    public override void Aplicar(GameObject bola)
    {
        // Este comodín no aplica un efecto directo aquí, el efecto es evaluado desde Banda
        Debug.Log("Comodín multiplicador activo. Efectos x2");
    }

    public override Color ObtenerColor()
    {
        return Color.magenta;
    }
}
