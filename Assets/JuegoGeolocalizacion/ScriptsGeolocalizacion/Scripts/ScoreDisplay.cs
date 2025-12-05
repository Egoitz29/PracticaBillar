using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    void Update()
    {
        if (GameSessionManager.Instance != null)
            scoreText.text = "PUNTOS: " + GameSessionManager.Instance.score;
        else
            scoreText.text = "PUNTOS: 0";
    }
}