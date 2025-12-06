using UnityEngine;
using UnityEngine.SceneManagement;

public class ZoneTrigger : MonoBehaviour
{
    public string[] minigames;
    public GameObject glowEffect;
    public GameObject interactButton;

    private bool playerInside = false;
    public bool visited = false;


    void Start()
    {
        glowEffect.SetActive(false);
        interactButton.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            glowEffect.SetActive(true);
            interactButton.SetActive(true);

            interactButton
                .GetComponent<UnityEngine.UI.Button>()
                .onClick.AddListener(EnterMinigame);

            // Zona visitada SOLO la primera vez
            if (!visited)
            {
                visited = true;
                GameSessionManager.Instance.ZoneVisited();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;

            glowEffect.SetActive(false);
            interactButton.SetActive(false);

            interactButton
                .GetComponent<UnityEngine.UI.Button>()
                .onClick.RemoveAllListeners();
        }
    }

    void EnterMinigame()
    {
        string scene = minigames[Random.Range(0, minigames.Length)];
        SceneManager.LoadScene(scene);
    }
}
