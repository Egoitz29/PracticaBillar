using CesiumForUnity;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class DeviceLocationService : MonoBehaviour
{
    public static DeviceLocationService Instance { get; private set; }
    public CesiumGlobeAnchor anchor;

    public bool Ready => Input.location.status == LocationServiceStatus.Running;
    public LocationInfo Data => Input.location.lastData;

    public float desiredAccuracy = 5f;
    public float updateDistance = 1f;
    public int startTimeoutSec = 20;

    public double forcedHeight = 592.0;

    public float smoothFactor = 0.25f;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    IEnumerator Start()
    {
#if UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation))
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
#endif
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("Localización deshabilitada por el usuario.");
            yield break;
        }

        Input.compass.enabled = true;
#if UNITY_IOS
        updateDistance = 0f; // iOS ignora updateDistance > 0
#endif
        Input.location.Start(desiredAccuracy, updateDistance);

        int t = 0;
        while (Input.location.status == LocationServiceStatus.Initializing && t < startTimeoutSec)
        { yield return new WaitForSeconds(1f); t++; }

        if (Input.location.status != LocationServiceStatus.Running)
        {
            Debug.LogError("No se pudo iniciar LocationService.");
            yield break;
        }
    }

    void Update()
    {
        if (anchor == null || !Ready) return;

        var d = Data;

        // Usamos solo lat/lon del GPS, altura fija
        double3 target = new double3(d.longitude, d.latitude, forcedHeight);

        anchor.longitudeLatitudeHeight =
            math.lerp(anchor.longitudeLatitudeHeight, target, smoothFactor);
    }
}