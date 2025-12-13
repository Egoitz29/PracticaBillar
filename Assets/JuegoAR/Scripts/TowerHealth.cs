using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth;

    private bool isDestroyed = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (UIManagerVR.Instance != null)
            UIManagerVR.Instance.UpdateTowerHealth(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        if (isDestroyed) return;

        currentHealth -= amount;

        if (currentHealth < 0)
            currentHealth = 0;

        if (UIManagerVR.Instance != null)
            UIManagerVR.Instance.UpdateTowerHealth(currentHealth);

        if (currentHealth <= 0)
        {
            isDestroyed = true;
            GameManagerVR.Instance.GameOver();
        }
    }
}
