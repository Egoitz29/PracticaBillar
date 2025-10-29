using UnityEngine;

[CreateAssetMenu(menuName = "Jokers/Joker Multi Rebotes")]

public class JokerMultiRebotes : Joker
{
    public override void AplicarEfecto(GameManager gameManager)
    {
        int totalRebotes = 0;

        // Asegúrate de que GameManager tenga una forma de acceder a las bolas
        foreach (var bola in gameManager.ObtenerTodasLasBolas())
        {
            if (bola.haTocadoBlanca)
            {
                totalRebotes = bola.rebotes;
            }
        }

        // Si no hubo rebotes, no multiplica nada
        if (totalRebotes > 0)
        {
            float multiplicador =  totalRebotes; // Ejemplo: 3 rebotes = x4
            gameManager.MultiplicarPuntaje(multiplicador);
            Debug.Log($"[JokerMultiRebotes] Multiplicador aplicado x{multiplicador} por {totalRebotes} rebotes.");
        }
    }
}
