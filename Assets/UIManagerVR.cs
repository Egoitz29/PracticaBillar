using UnityEngine;
using UnityEngine.UI;

public class UIManagerVR : MonoBehaviour
{
    public static UIManagerVR Instance;

    public Text coinsText;
    public Text roundText;
    public Text towerHealthText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void UpdateCoins(int amount)
    {
        coinsText.text = "Monedas: " + amount;
    }

    public void UpdateRound(int round)
    {
        roundText.text = "Ronda: " + round;
    }

    public void UpdateTowerHealth(float hp)
    {
        towerHealthText.text = "Torre: " + hp;
    }
}
