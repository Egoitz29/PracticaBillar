using UnityEngine;
using UnityEngine.EventSystems;

public class InventarioSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        var dragged = eventData.pointerDrag?.GetComponent<DraggableJokerInventario>();
        if (dragged == null) return;

        Transform origen = dragged.originalParent;   // Slot desde donde venía
        Transform destino = transform;               // Slot donde se suelta

        // Si el slot destino está vacío → mover normalmente
        if (destino.childCount == 0)
        {
            dragged.transform.SetParent(destino);
            dragged.transform.localPosition = Vector3.zero;
            dragged.originalParent = destino;
            Debug.Log($"✅ Movido: {dragged.name} → {destino.name}");
        }
        else
        {
            // Si ya hay otro comodín → intercambio de posiciones
            Transform otro = destino.GetChild(0);

            // Guardar temporalmente las posiciones
            otro.SetParent(origen);
            otro.localPosition = Vector3.zero;

            dragged.transform.SetParent(destino);
            dragged.transform.localPosition = Vector3.zero;

            // Actualizar referencias de padres
            var dragOtro = otro.GetComponent<DraggableJokerInventario>();
            if (dragOtro != null) dragOtro.originalParent = origen;
            dragged.originalParent = destino;

            Debug.Log($"🔄 Intercambio: {dragged.name} ↔ {otro.name}");
        }
    }
}
