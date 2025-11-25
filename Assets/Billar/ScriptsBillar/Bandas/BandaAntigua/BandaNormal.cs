using UnityEngine;
public class BandaNormal : BandaEfecto
{
    public override void AplicarEfecto(ContadorRebotesAntesDeBlanca contador)
    {
        contador.ContarRebote();
        Debug.Log("Banda Normal: Rebote contado. Total rebotes: " + contador.rebotes);
    }
}
