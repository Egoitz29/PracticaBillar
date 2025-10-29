using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableJoker : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector] public Joker jokerData;
    [HideInInspector] public int precio;
    [HideInInspector] public ShopJokerManager shop;

    private Transform originalParent;
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Canvas mainCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(mainCanvas.transform, true);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Detectar si soltó encima del inventario
        if (RectTransformUtility.RectangleContainsScreenPoint(shop.inventoryPanel.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera))
        {
            shop.ComprarPorArrastre(jokerData, precio);
            Destroy(gameObject); // se quita de la tienda al comprar
        }
        else
        {
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}
