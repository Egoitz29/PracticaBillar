using UnityEngine;
using System.Linq;

public class ButtonManager : MonoBehaviour
{
    private GameObject panel1;
    private GameObject panel2;
    private GameObject bolaColor;
    private GameObject bolaColorPosition;
    private GameObject bolaBlanca;
    private GameObject bolaBlancaPosition;

    private GameManager gameManager;

    void Start()
    {
        ReasignarVariables();
    }

    public void MostrarPanel2()
    {
        Debug.Log("✅ Botón 'Next Level' pulsado: ejecutando MostrarPanel2()");

        if (panel1 != null)
        {
            Debug.Log("Ocultando Panel1: " + panel1.name);
            panel1.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠️ Panel1 es NULL");
        }

        if (panel2 != null)
        {
            Debug.Log("Mostrando Panel2: " + panel2.name);
            panel2.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Panel2 es NULL");
        }
    }

    public void MostrarPanel1()
    {
        if (panel1 != null) panel1.SetActive(true);
    }

    public void SiguienteNivel()
    {
        if (panel2 != null) panel2.SetActive(false);
        gameManager.SiguienteNivel();
    }

    public void ReasignarVariables()
    {
        gameManager = GetComponent<GameManager>();

        panel1 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Panel1 (Tienda)");
        panel2 = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Panel2 (Niveles)");
        bolaColor = GameObject.Find("Bola Color");
        bolaColorPosition = GameObject.Find("Bola Color Posicion");
        bolaBlanca = GameObject.Find("Bola Blanca");
        bolaBlancaPosition = GameObject.Find("Bola Blanca Posicion");

        if (panel1 != null) panel1.SetActive(false); else Debug.LogWarning("Panel1 no encontrado");
        if (panel2 != null) panel2.SetActive(false); else Debug.LogWarning("Panel2 no encontrado");
        if (bolaColor == null) Debug.LogWarning("BolaColor no encontrado");
        if (bolaColorPosition == null) Debug.LogWarning("BolaColorPosition no encontrado");
        if (bolaBlanca == null) Debug.LogWarning("BolaBlanca no encontrado");
        if (bolaBlancaPosition == null) Debug.LogWarning("BolaBlancaPosition no encontrado");
    }

    public void ReiniciarPosiciones()
    {
        if (bolaColor != null && bolaColorPosition != null)
            bolaColor.transform.position = bolaColorPosition.transform.position;

        if (bolaBlanca != null && bolaBlancaPosition != null)
            bolaBlanca.transform.position = bolaBlancaPosition.transform.position;

        // Si hay rigidbody o física:
        Rigidbody2D rbColor = bolaColor?.GetComponent<Rigidbody2D>();
        if (rbColor != null) rbColor.linearVelocity = Vector2.zero;

        Rigidbody2D rbBlanca = bolaBlanca?.GetComponent<Rigidbody2D>();
        if (rbBlanca != null) rbBlanca.linearVelocity = Vector2.zero;
    }
}
