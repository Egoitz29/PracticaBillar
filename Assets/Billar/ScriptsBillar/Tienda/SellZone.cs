using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SellZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped == null)
        {
            Debug.Log("❌ Nada que vender");
            return;
        }

        var inventarioJoker = dropped.GetComponent<DraggableJokerInventario>();
        if (inventarioJoker == null)
        {
            Debug.Log($"⚠️ No puedes vender {dropped.name} porque no lo has comprado.");
            return;
        }

        var jokerData = inventarioJoker.jokerData;
        if (jokerData == null)
        {
            Debug.LogWarning("⚠️ Este comodín no tiene datos asignados (jokerData es NULL).");
            return;
        }

        // 🔗 Avisar al InventarioJokerManager
        var inventarioManager = FindObjectOfType<InventarioJokerManager>();
        if (inventarioManager != null)
            inventarioManager.VenderJoker(jokerData);

        // 🗑️ Eliminar objeto visual
        Destroy(dropped);

        // ✨ Feedback visual (flash blanco corto)
        var img = GetComponent<Image>();
        if (img != null)
        {
            Color baseColor = new Color(1, 0, 0, 0.4f);
            Color flash = Color.white;

            img.CrossFadeColor(flash, 0.1f, false, false);
            img.CrossFadeColor(baseColor, 0.25f, false, false);
        }
    }
}
