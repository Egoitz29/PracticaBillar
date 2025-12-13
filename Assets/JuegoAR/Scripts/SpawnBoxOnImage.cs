using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnBoxOnImage : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject boxPrefab;

    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    void OnDisable()
    {
        if (trackedImageManager != null)
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
            SpawnPrefab(trackedImage);

        foreach (var trackedImage in eventArgs.updated)
        {
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                if (!spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
                    SpawnPrefab(trackedImage);
                else
                    spawnedObjects[trackedImage.referenceImage.name].SetActive(true);
            }
            else
            {
                if (spawnedObjects.ContainsKey(trackedImage.referenceImage.name))
                    spawnedObjects[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }

    private void SpawnPrefab(ARTrackedImage trackedImage)
    {
        if (boxPrefab == null) return;

        GameObject obj = Instantiate(boxPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
        obj.transform.parent = trackedImage.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one * 0.05f;

        spawnedObjects[trackedImage.referenceImage.name] = obj;

        // Conectar al spawner
        TargetZone zone = obj.GetComponentInChildren<TargetZone>();
        TowerHealth health = obj.GetComponentInChildren<TowerHealth>();

        if (zone != null && health != null && CircularEnemySpawner.Instance != null)
        {
            CircularEnemySpawner.Instance.targetZone = zone;
            CircularEnemySpawner.Instance.towerHealth = health;
            CircularEnemySpawner.Instance.SetActive(true);
        }
    }
}
