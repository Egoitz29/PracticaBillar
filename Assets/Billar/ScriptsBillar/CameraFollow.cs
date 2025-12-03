using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float distance = 100f;
    public float height = 50f;

    public float rotSpeed = 90f;      // velocidad botones
    public float touchSensitivity = 0.3f; // sensibilidad dedo

    bool leftHeld, rightHeld;
    float angle;

    public void LeftDown() { leftHeld = true; }
    public void LeftUp() { leftHeld = false; }
    public void RightDown() { rightHeld = true; }
    public void RightUp() { rightHeld = false; }

    void LateUpdate()
    {
        if (!target) return;

        // ---------------------------
        // 1) INPUT DE BOTONES
        // ---------------------------
        float dir = (rightHeld ? 1f : 0f) - (leftHeld ? 1f : 0f);
        angle += dir * rotSpeed * Time.deltaTime;

        // ---------------------------
        // 2) INPUT DE DEDO (DRAG)
        // ---------------------------
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            // Solo rotamos si el dedo se mueve en horizontal
            if (t.phase == TouchPhase.Moved)
            {
                float delta = t.deltaPosition.x * touchSensitivity;
                angle += delta;
            }
        }

        // ---------------------------
        // 3) CÁLCULO DE POSICIÓN
        // ---------------------------
        Quaternion rot = Quaternion.Euler(0f, angle, 0f);
        Vector3 offset = rot * new Vector3(0f, 0f, -distance);

        Vector3 pos = target.position + offset + Vector3.up * height;
        transform.position = pos;

        // Mira al jugador
        transform.LookAt(target.position + Vector3.up * (height * 0.5f));
    }
}
