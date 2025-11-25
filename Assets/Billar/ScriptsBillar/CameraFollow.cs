using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;      // Player
    public float distance = 100;  // más lejos
    public float height = 50f;    // más alto
    public float rotSpeed = 90f;  // grados/seg

    bool leftHeld, rightHeld;
    float angle;
    public void LeftDown() { leftHeld = true; }
    public void LeftUp() { leftHeld = false; }
    public void RightDown() { rightHeld = true; }
    public void RightUp() { rightHeld = false; }

    void LateUpdate()
    {
        if (!target) return;

        float dir = (rightHeld ? 1f : 0f) - (leftHeld ? 1f : 0f);
        angle += dir * rotSpeed * Time.deltaTime;

        Quaternion rot = Quaternion.Euler(0f, angle, 0f);
        Vector3 offset = rot * new Vector3(0f, 0f, -distance);
        Vector3 pos = target.position + offset * 2f + Vector3.up * (height * 2f);

        transform.position = pos;
        transform.LookAt(target.position + Vector3.up * (height * 0.5f));
    }
}