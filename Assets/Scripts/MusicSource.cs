using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Obtener el componente AudioSource
        audioSource = GetComponent<AudioSource>();

        if (audioSource != null)
        {
            // Reproducir el audio
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No se ha encontrado el componente AudioSource");
        }
    }
}
