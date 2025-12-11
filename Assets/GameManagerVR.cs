using UnityEngine;

public class GameManagerVR : MonoBehaviour
{
    public static GameManagerVR Instance;

    public int coins = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UIManagerVR.Instance.UpdateCoins(coins);
    }

    public void ResetCoins()
    {
        coins = 0;
        UIManagerVR.Instance.UpdateCoins(coins);
    }
}
