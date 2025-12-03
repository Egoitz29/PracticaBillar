using UnityEngine;
using System.Collections;
using TMPro;

public class ZoneSpawner : MonoBehaviour
{
    public Transform player;
    public GameObject zonePrefab;
    public int zoneCount = 3;
    public float minDistance = 30f;
    public float maxDistance = 80f;

    public GameObject interactButton;

    public float stabilityRadius = 1f;
    public float stableTimeRequired = 5f;

    public TextMeshProUGUI gpsStatusText; // ← NUEVO

    bool zonesSpawned = false;

    IEnumerator Start()
    {
        gpsStatusText.gameObject.SetActive(true);
        gpsStatusText.text = "Esperando señal GPS...";

        // 1) Esperar al GPS real
        while (!DeviceLocationService.Instance || !DeviceLocationService.Instance.Ready)
        {
            gpsStatusText.text = "GPS no disponible...";
            yield return null;
        }

        gpsStatusText.text = "Posición inestable...";

        // 2) Esperar posición estable
        yield return StartCoroutine(WaitForStablePosition());

        // 3) Ocultar indicador
        gpsStatusText.gameObject.SetActive(false);

        // 4) Instanciar zonas
        SpawnZones();
    }

    IEnumerator WaitForStablePosition()
    {
        float stableTimer = 0f;
        Vector3 lastPos = player.position;

        while (stableTimer < stableTimeRequired)
        {
            Vector3 currentPos = player.position;
            float dist = Vector3.Distance(lastPos, currentPos);

            if (dist > stabilityRadius)
            {
                stableTimer = 0f;
                lastPos = currentPos;
            }
            else
            {
                stableTimer += Time.deltaTime;
            }

            gpsStatusText.text =
                "Estabilizando señal... " +
                Mathf.Clamp01(stableTimer / stableTimeRequired).ToString("P0");

            yield return null;
        }
    }

    public void SpawnZones()
    {
        if (zonesSpawned) return;
        zonesSpawned = true;

        for (int i = 0; i < zoneCount; i++)
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            float distance = Random.Range(minDistance, maxDistance);

            Vector3 spawnPos = player.position +
                               new Vector3(randomDir.x, 0, randomDir.y) * distance;

            GameObject zone = Instantiate(zonePrefab, spawnPos, Quaternion.identity);

            ZoneTrigger trigger = zone.GetComponent<ZoneTrigger>();
            trigger.interactButton = interactButton;
        }
    }
}
