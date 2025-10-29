using UnityEngine;
using UnityEngine.EventSystems;

public class JokerIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public Joker datos;
    [HideInInspector] public JokerTooltip tooltip;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null && datos != null)
            tooltip.Mostrar(datos.nombre, datos.descripcion, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.Ocultar();
    }
}
