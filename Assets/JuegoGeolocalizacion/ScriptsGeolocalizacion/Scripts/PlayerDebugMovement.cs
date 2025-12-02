using UnityEngine;

public class PlayerDebugMovement : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(h, 0, v) * speed * Time.deltaTime;
        transform.position += move;
    }
}
