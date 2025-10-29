using System.Collections.Generic;
using UnityEngine;

public class DetectorDeAgujeros : MonoBehaviour
{
    public List<Transform> agujeros;
    public float radioAgujero;

    void FixedUpdate()
    {
        foreach (var bola in FindObjectsOfType<BolaFisica>())
        {
            foreach (var agujero in agujeros)
            {
                if (Vector3.Distance(bola.transform.position, agujero.position) < radioAgujero)
                {
                    Destroy(bola.gameObject);
                    break;
                }
            }
        }
    }
}
