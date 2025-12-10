using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SpawnBaseOnImage : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager trackedImageManager;

    [Header("Prefab to Spawn")]
    public GameObject arrowPrefab;

    // Guardamos los objetos instanciados por imagen
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
        // Para cada nueva imagen detectada
        foreach (var trackedImage in eventArgs.added)
        {
            SpawnPrefab(trackedImage);
        }

        // Para imágenes ya detectadas que cambian de estado
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
        if (arrowPrefab == null)
        {
            return;
        }

        // Instancia el prefab centrado en la imagen
        GameObject obj = Instantiate(arrowPrefab, trackedImage.transform.position, trackedImage.transform.rotation);
        obj.transform.parent = trackedImage.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;

        // Ajusta la escala según tu necesidad
        obj.transform.localScale = Vector3.one * 0.05f;

        // Guardamos el objeto para evitar duplicados
        spawnedObjects[trackedImage.referenceImage.name] = obj;
    }
}
