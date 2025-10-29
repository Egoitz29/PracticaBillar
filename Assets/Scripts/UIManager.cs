using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoPuntosMeta;
    public TextMeshProUGUI textoPuntosAcumulados;
    public TextMeshProUGUI textoPuntosTurno;
    public TextMeshProUGUI textoTirosRestantes;
    public TextMeshProUGUI textoRondaActual;
    public TextMeshProUGUI textoOro;
    public Button botonOpciones;

    // Estado del juego (puedes pasar esto desde otro controlador)
    public int puntosMeta;
    public int puntosAcumulados;
    public int puntosTurno;
    public int tirosRestantes;
    public int rondaActual;
    public int oro;

    void Start()
    {
        botonOpciones.onClick.AddListener(AbrirMenuOpciones);
        ActualizarHUD();
    }

    void Update()
    {
        ActualizarHUD();
    }

    public void ActualizarHUD()
    {
        textoPuntosMeta.text = "Meta: " + puntosMeta;
        textoPuntosAcumulados.text = "Acumulados: " + puntosAcumulados;
        textoPuntosTurno.text = "Turno: " + puntosTurno;
        textoTirosRestantes.text = "Tiros: " + tirosRestantes;
        textoRondaActual.text = "Ronda: " + rondaActual;
        textoOro.text = "Oro: " + oro;
    }

    void AbrirMenuOpciones()
    {
        Debug.Log("Menú de opciones abierto");
        //Añadir menu opciones
    }
}
