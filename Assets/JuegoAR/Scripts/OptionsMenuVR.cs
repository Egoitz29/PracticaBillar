using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenuVR : MonoBehaviour
{
    [Header("Panel Opciones")]
    public GameObject optionsPanel;

    private bool isPaused = false;

    void Start()
    {
        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    // ---------- ABRIR OPCIONES ----------
    public void OpenOptions()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (optionsPanel != null)
            optionsPanel.SetActive(true);
    }

    // ---------- VOLVER AL JUEGO ----------
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (optionsPanel != null)
            optionsPanel.SetActive(false);
    }

    // ---------- IR AL MENÚ ----------
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
