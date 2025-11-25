using UnityEngine;

public class Zone : MonoBehaviour
{
    private ZoneManager manager;
    private int zoneIndex;

    public void Init(ZoneManager manager, int index)
    {
        this.manager = manager;
        this.zoneIndex = index;
        gameObject.SetActive(false); // Se activa solo cuando esté desbloqueada
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && manager != null && zoneIndex == manager.currentZoneIndex)
        {
            manager.StartMinigame();
            manager.OnZoneCompleted();
        }
    }
}
