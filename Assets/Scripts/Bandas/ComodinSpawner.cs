using UnityEngine;

public class ComodinSpawner : MonoBehaviour
{
    public GameObject[] comodinPrefabs; // Asigna los prefabs de ComodinPuntosExtra, ComodinTiroExtra, ComodinMultiplicador
    public Transform zonaSpawn;         // Un Empty GameObject a la derecha de la pantalla
    public float espacio = 2f;

    private void Start()
    {
        for (int i = 0; i < comodinPrefabs.Length; i++)
        {
            Vector3 spawnPos = zonaSpawn.position + new Vector3(0, -i * espacio, 0);
            Instantiate(comodinPrefabs[i], spawnPos, Quaternion.identity);
        }
    }
}
