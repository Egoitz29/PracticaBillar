using UnityEngine;

public class GameManagerVR : MonoBehaviour
{
    public static GameManagerVR Instance;

    [Header("Economía")]
    public int coins = 0;
    public int damageUpgradeCost = 10;

    [Header("Estado")]
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // Asegura que el juego nunca arranca pausado
        Time.timeScale = 1f;
        isGameOver = false;

        if (UIManagerVR.Instance != null)
        {
            UIManagerVR.Instance.UpdateCoins(coins);
            UIManagerVR.Instance.UpdateDamageCost(damageUpgradeCost);
        }

        if (PlayerStats.Instance != null && UIManagerVR.Instance != null)
        {
            UIManagerVR.Instance.UpdateDamage(PlayerStats.Instance.CurrentDamage);
        }
    }

    // ---------- ECONOMÍA ----------
    public void AddCoins(int amount)
    {
        if (isGameOver) return;

        coins += amount;

        if (UIManagerVR.Instance != null)
            UIManagerVR.Instance.UpdateCoins(coins);
    }

    public void ResetCoins()
    {
        coins = 0;

        if (UIManagerVR.Instance != null)
            UIManagerVR.Instance.UpdateCoins(coins);
    }

    public void TryUpgradeDamage()
    {
        if (isGameOver) return;
        if (RoundManager.Instance != null && RoundManager.Instance.isRoundActive)
            return;

        if (coins >= damageUpgradeCost && PlayerStats.Instance != null)
        {
            coins -= damageUpgradeCost;
            PlayerStats.Instance.UpgradeDamage();

            if (UIManagerVR.Instance != null)
            {
                UIManagerVR.Instance.UpdateCoins(coins);
                UIManagerVR.Instance.UpdateDamage(PlayerStats.Instance.CurrentDamage);
                UIManagerVR.Instance.UpdateDamageCost(damageUpgradeCost);
            }
        }
    }

    // ---------- GAME OVER ----------
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        // Parar rondas
        if (RoundManager.Instance != null)
            RoundManager.Instance.enabled = false;

        // Parar spawner
        CircularEnemySpawner spawner = FindObjectOfType<CircularEnemySpawner>();
        if (spawner != null)
            spawner.SetActive(false);

        // Pausar juego
        Time.timeScale = 0f;

        // Mostrar UI final
        if (UIManagerVR.Instance != null &&
            PlayerStats.Instance != null &&
            RoundManager.Instance != null)
        {
            UIManagerVR.Instance.ShowGameOver(
                RoundManager.Instance.currentRound,
                coins,
                PlayerStats.Instance.CurrentDamage
            );
        }
    }
}
