using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [Header("Botones de la UI")]
    public Button botonReintentar;   // Botón para reiniciar la partida
    public Button botonSalirMenu;    // Botón para volver al menú principal

    private void Start()
    {
        // Asignamos las funciones a los botones
        if (botonReintentar != null)
            botonReintentar.onClick.AddListener(ReiniciarPartida);

        if (botonSalirMenu != null)
            botonSalirMenu.onClick.AddListener(VolverAlMenu);

        Debug.Log(" GameOverUI listo.");
    }

    /// <summary>
    /// Reinicia completamente el juego, eliminando el GameManager y recargando la escena principal.
    /// </summary>
    public void ReiniciarPartida()
    {
        Debug.Log(" Reiniciando partida desde GameOver...");

        //  1. Destruye el GameManager si aún existe
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        //  2. Recarga la escena principal del juego desde cero
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);

        Debug.Log(" Juego reiniciado completamente.");
    }

    /// <summary>
    /// Carga el menú principal.
    /// </summary>
    public void VolverAlMenu()
    {
        Debug.Log(" Volviendo al menú principal...");

        // Limpia el GameManager si existe
        if (GameManager.Instance != null)
        {
            Destroy(GameManager.Instance.gameObject);
        }

        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }
}
