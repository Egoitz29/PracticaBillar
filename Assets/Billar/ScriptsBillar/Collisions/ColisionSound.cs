using UnityEngine;

public class BolaSonidoColision : MonoBehaviour
{
    public AudioClip sonidoColision;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
    }

    // Este método será llamado desde BolaCollisionHandler
    public void ReproducirSonidoColision(float velocidadMedia)
    {
        if (sonidoColision == null) return;

        float volumen = Mathf.Clamp01(velocidadMedia / 5f); // Ajusta el divisor para controlar el rango del volumen
        audioSource.PlayOneShot(sonidoColision, volumen);
    }
}
