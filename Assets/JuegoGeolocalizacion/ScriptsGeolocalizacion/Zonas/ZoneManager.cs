using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public int numberOfZones = 3;          // Número de zonas
    public Vector3 mapMin;                 // Límite mínimo del área de generación
    public Vector3 mapMax;                 // Límite máximo del área de generación
    public float zoneRadius = 5f;          // Radio de la zona

    [HideInInspector]
    public Transform[] zones;              // Zonas generadas
    public int currentZoneIndex = 0;      // Zona desbloqueada
    private bool minigameActive = false;

    public GameObject zonePrefab;          // Prefab de la zona (puede ser un cubo con collider trigger invisible)

    void Start()
    {
        GenerateZones();
    }

    void GenerateZones()
    {
        zones = new Transform[numberOfZones];

        for (int i = 0; i < numberOfZones; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(mapMin.x, mapMax.x),
                Random.Range(mapMin.y, mapMax.y),
                Random.Range(mapMin.z, mapMax.z)
            );

            GameObject zone = Instantiate(zonePrefab, randomPos, Quaternion.identity);
            zone.transform.localScale = Vector3.one * zoneRadius * 2; // Ajusta tamaño del área
            zone.GetComponent<Zone>().Init(this, i); // Asociamos al manager
            zones[i] = zone.transform;
        }

        // Solo la primera zona estará activa al inicio
        UnlockZone(0);
    }

    public void UnlockZone(int index)
    {
        for (int i = 0; i < zones.Length; i++)
        {
            zones[i].gameObject.SetActive(i == index);
        }
        currentZoneIndex = index;
    }

    public void OnZoneCompleted()
    {
        minigameActive = false;
        currentZoneIndex++;
        if (currentZoneIndex < zones.Length)
        {
            UnlockZone(currentZoneIndex);
        }
        else
        {
            Debug.Log("¡Todas las zonas completadas!");
        }
    }

    public void StartMinigame()
    {
        minigameActive = true;
        Debug.Log("Minijuego iniciado en zona " + currentZoneIndex);
        // Aquí llamarías a tu sistema de minijuego
    }
}
