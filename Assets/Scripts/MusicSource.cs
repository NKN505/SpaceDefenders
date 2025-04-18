using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Start()
    {
        // Intentar obtener el componente AudioSource del objeto
        audioSource = GetComponent<AudioSource>();

        // Si no hay un AudioSource, lo añadimos automáticamente
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            Debug.LogWarning("No se encontró un AudioSource en el objeto, se ha añadido uno automáticamente.");
        }

        // Reproducir el audio si se ha encontrado o añadido el AudioSource
        if (audioSource != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No se ha encontrado o añadido un componente AudioSource.");
        }
    }
}
