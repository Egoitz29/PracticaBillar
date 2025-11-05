using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    private GameObject panel1;
    private GameObject bolaColor;
    private GameObject bolaColorPosition;
    private GameObject bolaBlanca;
    private GameObject bolaBlancaPosition;
    private GameManager gameManager;

    [Header("Panel del menú")]
    public GameObject panelMenuExtra;

    void Start()
    {
        ReasignarVariables();

        if (panelMenuExtra != null)
            panelMenuExtra.SetActive(false);
    }

    // ✅ Botón "Juego 1"
    public void CargarJuego1()
    {
        Debug.Log("🎮 Cargando escena: Juego1...");
        SceneManager.LoadScene("Juego1");
    }

    // ✅ Botón "Opciones"
    public void BotonOpciones()
    {
        if (panelMenuExtra == null)
        {
            Debug.LogWarning("⚠️ PanelMenuExtra no asignado en el inspector.");
            return;
        }

        bool nuevoEstado = !panelMenuExtra.activeSelf;
        panelMenuExtra.SetActive(nuevoEstado);
        Debug.Log(nuevoEstado ? "📜 Panel de opciones abierto." : "❌ Panel de opciones cerrado.");
    }

    // ✅ Botón "Cerrar panel" dentro de las opciones
    public void CerrarPanelOpciones()
    {
        if (panelMenuExtra == null)
        {
            Debug.LogWarning("⚠️ PanelMenuExtra no asignado en el inspector.");
            return;
        }

        panelMenuExtra.SetActive(false);
        Debug.Log("❌ Panel de opciones cerrado manualmente.");
    }

    // ✅ Botón "Volver" dentro del panel
    public void VolverAlMenu()
    {
        Debug.Log("🔙 Volviendo al menú principal...");
        SceneManager.LoadScene("Menu"); // Cambia por el nombre exacto de tu escena de menú
    }

    public void ContinuarDesdeTienda()
    {
        Debug.Log("✅ [ButtonManager] Continuar desde tienda pulsado.");

        if (panel1 != null)
        {
            panel1.SetActive(false);
            Debug.Log("🛒 Tienda cerrada.");
        }

        ReiniciarPosiciones();

        gameManager = GameManager.Instance;
        if (gameManager == null)
        {
            Debug.LogWarning("⚠️ GameManager no encontrado.");
            return;
        }

        gameManager.SiguienteNivel();
    }

    public void MostrarPanel1()
    {
        StopAllMotion();
        if (panel1 != null)
        {
            panel1.SetActive(true);
            gameManager.tirosRestantes = 3;
            gameManager.uiManager?.ActualizarHUD();
        }
    }

    public void ReasignarVariables()
    {
        gameManager = GetComponent<GameManager>();
        panel1 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Panel1 (Tienda)");
        bolaColor = GameObject.Find("Bola Color");
        bolaColorPosition = GameObject.Find("Bola Color Posicion");
        bolaBlanca = GameObject.Find("Bola Blanca");
        bolaBlancaPosition = GameObject.Find("Bola Blanca Posicion");

        if (panel1 != null) panel1.SetActive(false);
    }

    public void ReiniciarPosiciones()
    {
        if (bolaColor != null && bolaColorPosition != null)
            bolaColor.transform.position = bolaColorPosition.transform.position;

        if (bolaBlanca != null && bolaBlancaPosition != null)
            bolaBlanca.transform.position = bolaBlancaPosition.transform.position;

        StopAllMotion();
    }

    private void StopAllMotion()
    {
        var all = FindObjectsOfType<Rigidbody2D>();
        foreach (var rb in all)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.Sleep();
        }
    }
}
