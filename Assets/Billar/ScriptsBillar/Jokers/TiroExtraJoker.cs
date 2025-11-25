using UnityEngine;

[CreateAssetMenu(menuName = "Jokers/Joker Extra Tiro")]
public class JokerExtraTiro : Joker
{
    public int tirosExtra = 1;

    public override void AplicarEfecto(GameManager gameManager)
    {
        gameManager.AddTiros(tirosExtra);
    }
}
