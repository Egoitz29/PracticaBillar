using UnityEngine;

[CreateAssetMenu(menuName = "Jokers/Joker x4")]
public class Jokerx4 : Joker
{
  

    public override void AplicarEfecto(GameManager gameManager)
    {
        gameManager.puntosJugador=gameManager.puntosJugador*4;
    }
}
