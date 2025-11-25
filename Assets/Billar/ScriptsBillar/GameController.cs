using CesiumForUnity;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] CesiumGlobeAnchor cesiumGlobeAnchor;
    [SerializeField] TextMeshProUGUI x;
    [SerializeField] TextMeshProUGUI y;
    [SerializeField] TextMeshProUGUI z;

    public DeviceLocationService gps;

    void Update()
    {
        // Longitud
        x.text = cesiumGlobeAnchor.longitudeLatitudeHeight.x.ToString("F6");
        // Latitud
        y.text = cesiumGlobeAnchor.longitudeLatitudeHeight.y.ToString("F6");
        // Altura
        z.text = cesiumGlobeAnchor.longitudeLatitudeHeight.z.ToString("F2");

    }
}
