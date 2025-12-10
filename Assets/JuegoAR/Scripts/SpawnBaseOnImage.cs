using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageArrowSpawner : MonoBehaviour
{
    public ARTrackedImageManager trackedImageManager;
    public GameObject arrowPrefab;

    private bool arrowSpawned = false;

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;
    }

    void OnImageChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (arrowSpawned) return;

        foreach (var image in args.added)
        {
            // Instanciar una flecha en relación a la imagen
            GameObject arrow = Instantiate(arrowPrefab, image.transform);
            arrowSpawned = true;
            break; // solo una flecha
        }
    }
}