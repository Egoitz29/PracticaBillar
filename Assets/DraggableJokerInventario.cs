using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableJokerInventario : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Joker1 jokerData;
    [HideInInspector] public Transform originalParent; // Slot original

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas mainCanvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent; // 🔗 Guarda el slot original
        transform.SetParent(mainCanvas.transform);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;

        // ✨ Efecto visual al empezar a arrastrar
        transform.localScale = Vector3.one * 1.15f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        transform.localScale = Vector3.one;

        GameObject dropZone = eventData.pointerCurrentRaycast.gameObject;

        // 🧩 Si no hay nada bajo el puntero, vuelve al slot original
        if (dropZone == null)
        {
            transform.SetParent(originalParent);
            rectTransform.localPosition = Vector3.zero;
            return;
        }

        // 🟩 Si se suelta sobre la zona de venta
        if (dropZone.CompareTag("ZonaVenta"))
        {
            InventarioJokerManager inventario = FindObjectOfType<InventarioJokerManager>();
            if (inventario != null)
            {
                inventario.VenderJoker(jokerData);
                Destroy(gameObject);
            }
            return;
        }

        // 🔹 Si no es un slot válido → vuelve al lugar original
        if (!dropZone.CompareTag("SlotInventario"))
        {
            transform.SetParent(originalParent);
            rectTransform.localPosition = Vector3.zero;
        }
    }

}
