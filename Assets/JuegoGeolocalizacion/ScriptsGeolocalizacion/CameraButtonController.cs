using UnityEngine;

public class CameraButtonController : MonoBehaviour
{
    public Transform player;           // Jugador o punto central
    public float rotationSpeed = 50f;  // Grados por segundo
    public Vector3 offset = new Vector3(0, 5, -10); // Posición relativa inicial

    private float horizontalInput = 0f;

    void Start()
    {
        // Posición inicial de la cámara
        transform.position = player.position + offset;
        transform.LookAt(player.position + Vector3.up * offset.y);
    }

    void Update()
    {
        // Si hay input, rotamos alrededor del jugador
        if (horizontalInput != 0f)
        {
            // Rotamos alrededor del jugador manteniendo altura
            transform.RotateAround(
                player.position + Vector3.up * offset.y, // centro del giro
                Vector3.up,                             // eje Y
                horizontalInput * rotationSpeed * Time.deltaTime
            );

            // Ajustamos altura constante
            Vector3 pos = transform.position;
            pos.y = player.position.y + offset.y;
            transform.position = pos;

            // Siempre mirar al jugador
            transform.LookAt(player.position + Vector3.up * offset.y);
        }
    }

    // Funciones para los botones
    public void RotateLeft()
    {
        horizontalInput = -1f;
    }

    public void RotateRight()
    {
        horizontalInput = 1f;
    }

    public void StopRotation()
    {
        horizontalInput = 0f;
    }
}
