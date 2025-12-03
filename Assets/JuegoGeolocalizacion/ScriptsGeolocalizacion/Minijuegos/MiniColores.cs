using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MiniColores : MonoBehaviour
{
    [Header("Tiempo por ronda")]
    public float timeLimit = 5f;
    private float timer;
    private bool finished = false;

    [Header("UI")]
    public TextMeshProUGUI colorText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI roundText;

    public Image backgroundPanel;

    [Header("Botones (asignar en Inspector)")]
    public Button[] colorButtons;

    [Header("Escena a la que vuelve")]
    public string returnSceneName = "Juego2";

    private string correctColor;
    private int currentRound = 1;
    private int totalRounds = 10;

    private string[] colorNames = { "ROJO", "VERDE", "AZUL", "AMARILLO" };
    private Color[] unityColors = { Color.red, Color.green, Color.blue, Color.yellow };

    void Start()
    {
        resultText.gameObject.SetActive(false);
        timer = timeLimit;

        foreach (var btn in colorButtons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }

        UpdateRoundText();
        GenerateRound();
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

    Color RandomPresetColor()
    {
        return unityColors[Random.Range(0, unityColors.Length)];
    }

    void GenerateRound()
    {
        correctColor = colorNames[Random.Range(0, colorNames.Length)];

        Color textColor = unityColors[Random.Range(0, unityColors.Length)];
        colorText.text = correctColor;
        colorText.color = textColor;

        RandomizeButtons();

        if (backgroundPanel != null)
        {
            Color bgColor;
            do
            {
                bgColor = RandomPresetColor();
            } while (bgColor == textColor); 

            backgroundPanel.color = bgColor;
        }

        timer = timeLimit;
    }

    void RandomizeButtons()
    {
        List<string> randomNames = new List<string>(colorNames);
        ShuffleList(randomNames);

        List<Color> randomVisualColors = new List<Color>(unityColors);
        ShuffleList(randomVisualColors);

        for (int i = 0; i < colorButtons.Length; i++)
        {
            var tmp = colorButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = randomNames[i];              
            tmp.color = randomVisualColors[i];     
        }

        ShuffleButtonOrder();
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(0, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    void ShuffleButtonOrder()
    {
        Transform parent = colorButtons[0].transform.parent;

        List<Button> temp = new List<Button>(colorButtons);
        ShuffleList(temp);

        for (int i = 0; i < temp.Count; i++)
        {
            temp[i].transform.SetSiblingIndex(i);
        }
    }

    void OnButtonClick(Button btn)
    {
        if (finished) return;

        string clickedName = btn.GetComponentInChildren<TextMeshProUGUI>().text;

        if (clickedName == correctColor)
        {
            NextRound();
        }
        else
        {
            Lose();
        }
    }

    void NextRound()
    {
        currentRound++;

        if (currentRound > totalRounds)
        {
            Win();
            return;
        }

        UpdateRoundText();
        GenerateRound();
    }

    void UpdateRoundText()
    {
        roundText.text = "Ronda: " + currentRound + " / " + totalRounds;
    }

    void Win()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        GameSessionManager.Instance.AddScore(10);
        resultText.text = "¡HAS GANADO!";
        
        Invoke("ReturnToMap", 1f);
    }

    void Lose()
    {
        finished = true;
        resultText.gameObject.SetActive(true);
        GameSessionManager.Instance.AddScore(-5);
        resultText.text = "HAS PERDIDO";
        Invoke("ReturnToMap", 1f);
    }

    void ReturnToMap()
    {
        SceneManager.LoadScene(returnSceneName);
    }
}
