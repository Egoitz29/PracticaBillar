using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Referencias")]
    public ShopJokerManager shop;     // Asigna tu ShopJokerManager en el Inspector

    [Header("Colores de feedback")]
    public Color idleColor = new Color(0.2f, 0.6f, 1f, 0.25f);   // normal
    public Color hoverColor = new Color(0.2f, 0.9f, 1f, 0.45f);  // al acercar
    public Color denyColor = new Color(1f, 0.3f, 0.3f, 0.45f);  // sin oro

    private Image bg;

    void Awake()
    {
        bg = GetComponent<Image>();
        if (bg != null) bg.color = idleColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var drag = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableJoker>() : null;
        if (drag == null || bg == null) return;

        bool puedeComprar = shop.gameManager.Oro >= drag.precio;
        bg.color = puedeComprar ? hoverColor : denyColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (bg != null) bg.color = idleColor;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (bg != null) bg.color = idleColor;

        var drag = eventData.pointerDrag ? eventData.pointerDrag.GetComponent<DraggableJoker>() : null;
        if (drag == null) return;

        if (shop.gameManager.Oro >= drag.precio)
        {
            shop.ComprarPorArrastre(drag.jokerData, drag.precio);
            Destroy(drag.gameObject);      // Quita el comodín de la tienda
        }
        else
        {
            Debug.Log("❌ No hay oro suficiente.");
        }
    }
}
