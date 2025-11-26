using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniRompeCubo2D : MonoBehaviour
{
    public int tapsNeeded = 3;
    public float timeLimit = 5f;

    public Button tapButton;
    public TextMeshProUGUI tapsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    private int currentTaps = 0;
    private float timer;
    private bool finished = false;

    // NOMBRE de la escena a la que volver (tu mapa GPS)
    public string returnSceneName = "MapaGPS";

    void Start()
    {
        timer = timeLimit;
        resultText.gameObject.SetActive(false);

        tapButton.onClick.AddListener(OnTap);
        UpdateUI();
    }

    void Update()
    {
        if (finished) return;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 0;
            Lose();
        }

        timerText.text = "Tiempo: " + timer.ToString("F1");
    }

    void OnTap()
    {
        if (finished) return;

        currentTaps++;
        UpdateUI();

        if (currentTaps >= tapsNeeded)
        {
            Win();
        }
    }

    void UpdateUI()
    {
        tapsText.text = "Golpes: " + currentTaps + " / " + tapsNeeded;
    }

    void Win()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        resultText.text = "HAS GANADO";
        Invoke("ReturnToMap", 1f); // vuelve al mapa en 1 segundo
    }

    void Lose()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        resultText.text = "HAS PERDIDO";
        Invoke("ReturnToMap", 1f); // vuelve al mapa en 1 segundo
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}
