using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI textoPuntosMeta;
    public TextMeshProUGUI textoPuntosAcumulados;
    public TextMeshProUGUI textoPuntosTurno;   // si no lo usas, puedes ocultarlo
    public TextMeshProUGUI textoTirosRestantes;
    public TextMeshProUGUI textoRondaActual;   // si no lo usas, puedes ocultarlo
    public TextMeshProUGUI textoOro;
    public Button botonOpciones;

    void Start()
    {
        if (botonOpciones) botonOpciones.onClick.AddListener(() => Debug.Log("Menú opciones"));
        ActualizarHUD(); // primer pintado
    }

    public void ActualizarHUD()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        if (textoPuntosMeta) textoPuntosMeta.text = "Meta: " + gm.puntosRequeridos;
        if (textoPuntosAcumulados) textoPuntosAcumulados.text = "Puntos: " + gm.puntosJugador;
        if (textoTirosRestantes) textoTirosRestantes.text = "Tiros: " + gm.tirosRestantes;
        if (textoOro) textoOro.text = "Oro: " + gm.Oro;
        // textoPuntosTurno / textoRondaActual: pinta lo que uses realmente
    }
}
