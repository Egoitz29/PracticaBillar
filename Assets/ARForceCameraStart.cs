using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Android;

public class ARForceCameraStart : MonoBehaviour
{
    public ARSession arSession;
    public Camera arCamera;

    void Awake()
    {
        Debug.Log("[ARForce] Awake");

        // 1. Forzar permiso de cámara
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("[ARForce] Requesting camera permission");
            Permission.RequestUserPermission(Permission.Camera);
        }
    }

    void Start()
    {
        Debug.Log("[ARForce] Start");

        // 2. Forzar ARSession
        if (arSession != null)
        {
            arSession.enabled = false;
            arSession.enabled = true;
            Debug.Log("[ARForce] ARSession restarted");
        }
        else
        {
            Debug.LogError("[ARForce] ARSession NULL");
        }

        // 3. Forzar cámara activa
        if (arCamera != null)
        {
            arCamera.gameObject.SetActive(true);
            Debug.Log("[ARForce] Camera forced active");
        }
        else
        {
            Debug.LogError("[ARForce] Camera NULL");
        }

        // 4. Forzar ARCameraBackground
        var bg = arCamera.GetComponent<ARCameraBackground>();
        if (bg != null)
        {
            bg.enabled = true;
            Debug.Log("[ARForce] ARCameraBackground enabled");
        }
        else
        {
            Debug.LogError("[ARForce] ARCameraBackground NOT FOUND");
        }
    }
}
