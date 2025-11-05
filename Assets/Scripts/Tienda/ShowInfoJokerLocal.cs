using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class ShowInfoJokerLocal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Joker1 joker;   // asignado al crear la carta

    private GameObject panelInfo;
    private RectTransform panelRT;
    private TextMeshProUGUI nombreText, descText, precioText;
    private Image iconoImg;
    private bool visible = false;

    void Start()
    {
        // Buscar el panel global (una sola vez)
        panelInfo = GameObject.Find("PanelInfoJoker");
        if (panelInfo == null)
        {
            Debug.LogWarning("⚠️ No se encontró PanelInfoJoker en la escena.");
            return;
        }

        panelRT = panelInfo.GetComponent<RectTransform>();
        nombreText = panelInfo.transform.Find("Nombre")?.GetComponent<TextMeshProUGUI>();
        descText = panelInfo.transform.Find("Descripcion")?.GetComponent<TextMeshProUGUI>();
        precioText = panelInfo.transform.Find("PrecioVenta")?.GetComponent<TextMeshProUGUI>();
        iconoImg = panelInfo.transform.Find("Fondo")?.GetComponent<Image>();

        panelInfo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (joker == null || panelInfo == null) return;
        MostrarPanel(eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panelInfo != null)
        {
            StopAllCoroutines();
            StartCoroutine(FadePanel(false));
        }
    }

    private void MostrarPanel(PointerEventData eventData)
    {
        visible = true;
        panelInfo.SetActive(true);

        // Rellenar info
        if (nombreText) nombreText.text = joker.nombre;
        if (descText) descText.text = joker.descripcion;
        if (precioText) precioText.text = $"💰 Valor venta: {joker.precioVenta}";
        if (iconoImg && joker.icono) iconoImg.sprite = joker.icono;

        // Posicionar cerca del cursor
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            panelRT.parent as RectTransform,
            eventData.position + new Vector2(80, -40),
            null,
            out pos);
        panelRT.localPosition = pos;

        StopAllCoroutines();
        StartCoroutine(FadePanel(true));
    }

    private IEnumerator FadePanel(bool show)
    {
        CanvasGroup cg = panelInfo.GetComponent<CanvasGroup>();
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
        if (!show) panelInfo.SetActive(false);
    }
}
