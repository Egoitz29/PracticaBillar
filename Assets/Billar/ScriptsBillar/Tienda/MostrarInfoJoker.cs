using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections;

public class MostrarInfoJoker : MonoBehaviour, IPointerClickHandler
{
    [Header("Panel de información")]
    public GameObject panelInfo;
    public TextMeshProUGUI nombreText;
    public TextMeshProUGUI descripcionText;
    public TextMeshProUGUI precioVentaText;
    public Image iconoImg;

    private Joker1 joker;
    private bool visible = false;
    private RectTransform panelRT;

    public void AsignarJoker(Joker1 nuevo)
    {
        joker = nuevo;
    }

    void Start()
    {
        if (panelInfo == null)
        {
            panelInfo = GameObject.Find("PanelInfoJoker");
            if (panelInfo == null)
            {
                Debug.LogWarning("⚠️ No se encontró el PanelInfoJoker en la escena.");
                return;
            }
        }

        panelRT = panelInfo.GetComponent<RectTransform>();
        panelInfo.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (joker == null || panelInfo == null)
            return;

        visible = !visible;
        panelInfo.SetActive(visible);

        if (visible)
        {
            nombreText.text = joker.nombre;
            descripcionText.text = joker.descripcion;
            precioVentaText.text = $"💰 Valor venta: {joker.precioVenta}";
            if (iconoImg && joker.icono) iconoImg.sprite = joker.icono;

            // Posicionar el panel junto al comodín
            Vector3 offset = new Vector3(220f, 0, 0); // ajusta distancia
            panelRT.position = transform.position + offset;

            StartCoroutine(FadePanel(true));
        }
        else
        {
            StartCoroutine(FadePanel(false));
        }
    }

    private IEnumerator FadePanel(bool show)
    {
        CanvasGroup cg = panelInfo.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = panelInfo.AddComponent<CanvasGroup>();

        float dur = 0.15f;
        float t = 0f;
        float start = show ? 0f : 1f;
        float end = show ? 1f : 0f;

        while (t < dur)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / dur);
            yield return null;
        }

        cg.alpha = end;
        if (!show)
            panelInfo.SetActive(false);
    }
}
