using UnityEngine;
using TMPro;

public class JokerTooltip : MonoBehaviour
{
    public TextMeshProUGUI texto;   // arrastra aquí el Text TMP
    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    public void Mostrar(string nombre, string descripcion, Vector3 posicion)
    {
        texto.text = $"<b>{nombre}</b>\n{descripcion}";
        rect.position = posicion + new Vector3(0, 120f, 0); // un poco por encima
        gameObject.SetActive(true);
    }

    public void Ocultar()
    {
        gameObject.SetActive(false);
    }
}
