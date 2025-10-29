using UnityEngine;

[CreateAssetMenu(menuName = "Jokers/Joker x4")]
public class JokerMulti4 : Joker
{

    public override void AplicarEfecto(GameManager gameManager)
    {
        int puntosNormales = 0;

        foreach (var bola in gameManager.ObtenerTodasLasBolas())
        {
            if (bola.haTocadoBlanca)
            {
                puntosNormales += bola.rebotes;
            }
        }

        gameManager.puntosJugador -= puntosNormales;         // Elimina puntos base
        gameManager.puntosJugador += puntosNormales * 4;     // Aplica multiplicador

        Debug.Log("normales"+ puntosNormales*4);    
    }
}
