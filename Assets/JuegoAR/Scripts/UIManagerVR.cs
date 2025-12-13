using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManagerVR : MonoBehaviour
{
    public static UIManagerVR Instance;

    [Header("Textos HUD")]
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI towerHealthText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI damageText;

    [Header("Textos Tienda")]
    public TextMeshProUGUI coinsTextShop;
    public TextMeshProUGUI damageCostText;

    [Header("Panel Tienda")]
    public GameObject upgradePanel;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI roundStatText;
    public TextMeshProUGUI coinsStatText;
    public TextMeshProUGUI damageStatText;

    [Header("Ronda")]
    public float roundDuration = 30f; // Duración de la primera ronda
    private float remainingRoundTime;
    private bool roundActive = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!roundActive) return;

        // Reducir tiempo restante
        remainingRoundTime -= Time.deltaTime;
        if (remainingRoundTime < 0)
            remainingRoundTime = 0;

        // Actualizar UI
        UpdateTimer(remainingRoundTime);

        // Fin de ronda
        if (remainingRoundTime <= 0)
        {
            EndRound();
        }
    }

    // ---------- HUD ----------
    public void UpdateCoins(int amount)
    {
        if (coinsText != null)
            coinsText.text = "Coins: " + amount;

        if (coinsTextShop != null)
            coinsTextShop.text = "Coins: " + amount;
    }

    public void UpdateRound(int round)
    {
        if (roundText != null)
            roundText.text = "Ronda: " + round;
    }

    public void UpdateDamage(float damage)
    {
        if (damageText != null)
            damageText.text = "Daño: " + Mathf.CeilToInt(damage);
    }

    public void UpdateDamageCost(int cost)
    {
        if (damageCostText != null)
            damageCostText.text = "Mejorar daño - Coste: " + cost;
    }

    public void UpdateTowerHealth(float hp)
    {
        if (towerHealthText != null)
            towerHealthText.text = "Torre: " + Mathf.CeilToInt(hp);
    }

    public void UpdateTimer(float time)
    {
        if (timerText != null)
            timerText.text = "Tiempo: " + Mathf.CeilToInt(time);
    }

    public void SetUpgradePanel(bool active)
    {
        if (upgradePanel != null)
            upgradePanel.SetActive(active);

        if (timerText != null)
            timerText.gameObject.SetActive(!active);
    }

    // ---------- RONDA ----------
    public void StartRound()
    {
        remainingRoundTime = roundDuration;
        roundActive = true;

        // Activar spawn de enemigos
        if (CircularEnemySpawner.Instance != null)
            CircularEnemySpawner.Instance.SetActive(true);
    }

    private void EndRound()
    {
        roundActive = false;

        // Desactivar spawn
        if (CircularEnemySpawner.Instance != null)
            CircularEnemySpawner.Instance.SetActive(false);

        Debug.Log("¡Ronda terminada!");
    }

    // ---------- GAME OVER ----------
    public void ShowGameOver(int round, int coins, float damage)
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (roundStatText != null)
            roundStatText.text = "Ronda alcanzada: " + round;

        if (coinsStatText != null)
            coinsStatText.text = "Monedas obtenidas: " + coins;

        if (damageStatText != null)
            damageStatText.text = "Daño final: " + Mathf.CeilToInt(damage);
    }

    // ---------- BOTONES ----------
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
