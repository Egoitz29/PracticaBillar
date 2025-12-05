using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MiniNoToquesNada2D : MonoBehaviour
{
    public float timeToWin = 3f;
    public Button tapButton;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI fakeCounterText;

    private float timer;
    private bool finished = false;
    public string returnSceneName = "MapaGPS";

    void Start()
    {
        timer = timeToWin;
        resultText.gameObject.SetActive(false);
        fakeCounterText.text = "Golpes: 0 / 3";
        tapButton.onClick.AddListener(PlayerTouched);
    }

    void Update()
    {
        if (finished) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            Win();
        }

        timerText.text = "Tiempo: " + timer.ToString("F1");
    }

    void PlayerTouched()
    {
        if (finished) return;
        fakeCounterText.text = "Golpes: " + Random.Range(1, 3) + " / 3";
        Lose();
    }

    void Win()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        resultText.text = "HAS GANADO";
        GameSessionManager.Instance.AddScore(10);
        Invoke("ReturnToMap", 1f);
    }

    void Lose()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        resultText.text = "HAS PERDIDO";
        GameSessionManager.Instance.AddScore(-5);
        Invoke("ReturnToMap", 1f);
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}