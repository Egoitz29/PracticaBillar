using UnityEngine;

public class BandaMultiplicadora : BandaEfecto
{
    public int factor = 2;

    public override void AplicarEfecto(ContadorRebotesAntesDeBlanca contador)
    {
        if (contador.haSidoLanzada && !contador.haTocadoBlanca)
        {

            contador.rebotes *= factor;
            Debug.Log($"Banda Multiplicadora: rebotes multiplicados x{factor}. Total: {contador.rebotes}");
        }
    }
}
