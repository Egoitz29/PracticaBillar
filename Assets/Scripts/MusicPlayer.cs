using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource audioSource;

    void Start()
    {
        audioSource.loop = true;
        audioSource.Play();
    }
}
