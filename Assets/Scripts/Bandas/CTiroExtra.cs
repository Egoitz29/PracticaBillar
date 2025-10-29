using UnityEngine;

public class CTiroExtra : Comodin
{
    int cantidad = 1;
    public override void Aplicar(GameObject bola)
    {
        // Supone que hay un GameManager que gestiona los tiros
        GameManager.Instance.AddTiros(cantidad);
        Debug.Log("Se ha sumado un tiro extra");
    }

    public override Color ObtenerColor()
    {
        return Color.green;
    }
}
