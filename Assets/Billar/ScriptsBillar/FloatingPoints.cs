using UnityEngine;
using TMPro;

public class FloatingPoints : MonoBehaviour
{
    public float floatSpeed = 1f;
    public float duration = 1f;
    public Vector3 floatOffset = new Vector3(0, 1, 0);
    private TextMeshPro textMesh;
    private float timer = 0f;

    public void Initialize(int points)
    {
        textMesh = GetComponentInChildren<TextMeshPro>();
        textMesh.text = "+" + points.ToString();
    }

    void Update()
    {
        transform.position += floatOffset * floatSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}
