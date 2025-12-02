using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniBotonFalso2D : MonoBehaviour
{
    public float timeToWin = 3f;

    public Button tapCatcherFullscreen;
    public Button fakeButton;

    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;

    private float timer;
    private bool finished = false;
    private bool gameStarted = false;

    public string returnSceneName = "MapaGPS";

    // Nuevo: tiempo aleatorio para mostrar el botón
    private float randomAppearTime;
    public float fakeButtonShowTime = 0.7f; // cuanto dura visible

    void Start()
    {
        gameStarted = false;

        timer = timeToWin;
        resultText.gameObject.SetActive(false);
        fakeButton.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        tutorialText.gameObject.SetActive(true);
        tutorialText.text = "No siempre hacer caso es ganar...";

        tapCatcherFullscreen.onClick.AddListener(PlayerTouched);
        fakeButton.onClick.AddListener(PlayerTouched);

        // Elegimos un momento aleatorio entre 0.5 y timeToWin - 0.5
        randomAppearTime = Random.Range(0.5f, timeToWin - 0.5f);

        Invoke("StartRealGame", 1f);
    }

    void StartRealGame()
    {
        tutorialText.gameObject.SetActive(false);
        instructionText.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        instructionText.text = "PULSA EL BOTÓN PARA GANAR";

        gameStarted = true;
    }

    void Update()
    {
        if (!gameStarted || finished) return;

        timer -= Time.deltaTime;

        // Momento exacto donde aparece el botón falso
        if (timer <= (timeToWin - randomAppearTime) && !fakeButton.gameObject.activeSelf)
        {
            ShowFakeButton();
        }

        if (timer <= 0)
        {
            timer = 0;
            Win();
        }

        timerText.text = "Tiempo: " + timer.ToString("F1");
    }

    void ShowFakeButton()
    {
        fakeButton.gameObject.SetActive(true);
        Invoke("HideFakeButton", fakeButtonShowTime);
    }

    void HideFakeButton()
    {
        if (!finished)
            fakeButton.gameObject.SetActive(false);
    }

    void PlayerTouched()
    {
        if (!gameStarted || finished) return;
        Lose();
    }

    void Win()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        GameSessionManager.Instance.AddScore(10);
        resultText.text = "Exacto. A veces no hacer nada es la mejor decisión.";
        Invoke("ReturnToMap", 3f);
    }

    void Lose()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        GameSessionManager.Instance.AddScore(-5);
        resultText.text = "Demasiado impulsivo… esta vez era NO tocar.";
        Invoke("ReturnToMap", 3f);
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}
