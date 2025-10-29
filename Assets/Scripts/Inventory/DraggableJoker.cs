using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableJoker : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Joker jokerData;        // Datos del comodín (ScriptableObject)
    [HideInInspector] public int precio;             // Precio asignado desde la tienda
    [HideInInspector] public ShopJokerManager shop;  // Referencia al manager principal
    [HideInInspector] public JokerTooltip tooltip;   // Referencia al panel de descripción

    private Transform originalParent;    // Donde estaba antes de arrastrar
    private CanvasGroup canvasGroup;     // Controla visibilidad y raycasts
    private RectTransform rectTransform;
    private Canvas mainCanvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
    }

    // 🔹 Cuando empieza el arrastre
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;              // Guarda el panel original
        transform.SetParent(mainCanvas.transform, true); // Lo mueve al frente
        canvasGroup.blocksRaycasts = false;              // Permite que el inventario detecte el drop
        canvasGroup.alpha = 0.8f;                        // Un poco transparente al arrastrar
    }

    // 🔹 Mientras arrastras
    public void OnDrag(PointerEventData eventData)
    {
        // Mueve el comodín con el dedo o el ratón
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    // 🔹 Cuando sueltas
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        // Si no se soltó en zona válida, vuelve a su sitio
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = Vector2.zero;
    }

    // 🔹 Mostrar tooltip al pasar el ratón o tocar
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null && jokerData != null)
        {
            tooltip.Mostrar(
                $"{jokerData.nombre}  ({precio} oro)",
                jokerData.descripcion,
                transform.position
            );
        }
    }

    // 🔹 Ocultar tooltip al salir
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Ocultar();
    }
}
