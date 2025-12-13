using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public float baseDamage = 10f;
    public float damagePerUpgrade = 5f;
    public int damageLevel = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public float CurrentDamage
    {
        get { return baseDamage + damageLevel * damagePerUpgrade; }
    }

    public void UpgradeDamage()
    {
        damageLevel++;
    }
}
