using UnityEngine;
using TMPro;

public class FinCarreraUI : MonoBehaviour
{
    public TextMeshProUGUI tiempoText;
    public TextMeshProUGUI distanciaText;
    public TextMeshProUGUI puntuacionText;

    void Start()
    {
        tiempoText.text = GameSessionManager.Instance.totalTime.ToString("F1") + " s";
        distanciaText.text = Mathf.Round(GameSessionManager.Instance.totalDistance) + " m";
        puntuacionText.text = GameSessionManager.Instance.score.ToString();
    }

    public void VolverAlMapa()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Juego2");
    }
}
