using UnityEngine;

public class ZoneSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject zonePrefab;
    public int zoneCount = 3;
    public float minDistance = 30f;
    public float maxDistance = 80f;

    public GameObject interactButton;   // ← botón global del HUD

    void Start()
    {
        SpawnZones();
    }

    public void SpawnZones()
    {
        for (int i = 0; i < zoneCount; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);

            Vector3 spawnPos = player.position +
                               new Vector3(randomDir.x, 0, randomDir.y) * distance;

            GameObject zone = Instantiate(zonePrefab, spawnPos, Quaternion.identity);

            // ← asignar el botón al script ZoneTrigger
            ZoneTrigger trigger = zone.GetComponent<ZoneTrigger>();
            trigger.interactButton = interactButton;
        }
    }
}
