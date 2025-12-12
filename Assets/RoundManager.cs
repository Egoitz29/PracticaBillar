using UnityEngine;

public class RoundManager : MonoBehaviour
{
    public static RoundManager Instance;

    [Header("Rondas")]
    public int currentRound = 1;
    public float roundDuration = 60f;

    private float timer;
    public bool isRoundActive = false;

    [Header("Referencias")]
    public CircularEnemySpawner spawner;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // 🔹 Empezamos DIRECTAMENTE en la ronda 1 (sin tienda)
        StartRound();
    }

    void Update()
    {
        if (!isRoundActive) return;

        timer -= Time.deltaTime;

        UIManagerVR.Instance.UpdateTimer(timer); // 👈 ESTA ES LA CLAVE

        if (timer <= 0f)
        {
            EndRound();
        }
    }


    // ▶️ Arranca una ronda
    void StartRound()
    {
        isRoundActive = true;
        timer = roundDuration;

        UIManagerVR.Instance.SetUpgradePanel(false);
        UIManagerVR.Instance.UpdateRound(currentRound);

        spawner.SetActive(true);
    }

    // ▶️ Botón "Empezar Ronda" (desde la tienda)
    public void StartNextRound()
    {
        StartRound();
    }

    void EndRound()
    {
        isRoundActive = false;

        spawner.SetActive(false);
        UIManagerVR.Instance.SetUpgradePanel(true);

        currentRound++;
        UIManagerVR.Instance.UpdateRound(currentRound);
    }
}
