using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnBaseOnImage : MonoBehaviour
{
    public ARTrackedImageManager imageManager;
    public GameObject basePrefab;

    private GameObject spawnedBase;

    void OnEnable()
    {
        imageManager.trackedImagesChanged += OnChanged;
    }

    void OnDisable()
    {
        imageManager.trackedImagesChanged -= OnChanged;
    }

    void OnChanged(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            if (spawnedBase == null)
            {
                spawnedBase = Instantiate(basePrefab, trackedImage.transform.position, trackedImage.transform.rotation);
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (spawnedBase != null)
            {
                spawnedBase.transform.position = trackedImage.transform.position;
                spawnedBase.transform.rotation = trackedImage.transform.rotation;
            }
        }
    }
}
