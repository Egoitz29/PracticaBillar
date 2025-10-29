using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Este script gestiona los botones y acciones de la escena GameOver
public class GameOverUI : MonoBehaviour
{
    [Header("Botones de la UI")]
    public Button botonReintentar;   // Botón para reiniciar la partida
    public Button botonSalirMenu;    // Botón para volver al menú principal (si lo tienes)

    private void Start()
    {
        // Asignamos las funciones a los botones si existen
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReiniciarPartida);

        if (botonSalirMenu != null)
            botonSalirMenu.onClick.AddListener(VolverAlMenu);
    }

    /// <summary>
    /// Reinicia completamente la partida y vuelve a cargar la escena principal del juego.
    /// </summary>
    public void ReiniciarPartida()
    {
        Debug.Log(" Reiniciando partida desde GameOver...");

        //  Antes de cargar la escena, reiniciamos el estado del GameManager
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReiniciarEstado();
        }

        // Carga la escena principal (asegúrate de que el nombre coincide)
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Vuelve al menú principal (si tienes una escena de menú).
    /// </summary>
    public void VolverAlMenu()
    {
        Debug.Log(" Volviendo al menú principal...");
        SceneManager.LoadScene("MenuPrincipal"); // Cambia el nombre por el de tu menú si es distinto
    }
}
