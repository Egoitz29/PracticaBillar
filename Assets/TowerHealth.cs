using UnityEngine;

public class TowerHealth : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        UIManagerVR.Instance.UpdateTowerHealth(currentHealth);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UIManagerVR.Instance.UpdateTowerHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Debug.Log("La torre ha sido destruida.");
            // Aquí puedes poner Game Over
        }
    }
}
