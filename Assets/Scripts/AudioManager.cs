using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Necesario para IEnumerator

public class AudioManager : MonoBehaviour
{
    // ======== Singleton pattern
    public static AudioManager Instance;

    // ======== Configuración de audio
    public AudioClip menuMusic;
    public AudioClip gameMusic;
    [Range(0f, 1f)] public float volume = 0.5f;
    public float fadeDuration = 1.0f; // Duración del fade en segundos

    private AudioSource audioSource;
    private string[] menuScenes = { "Portada", "Intro", "Creditos", "Top10" };
    private Coroutine activeFadeCoroutine; // Referencia al fade activo

    // -------- Inicialización singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // -------- Configura el componente de audio
    private void InitializeAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    // -------- Detecta cambios de escena CON FADE
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool isMenuScene = System.Array.Exists(menuScenes, x => x == scene.name);
        AudioClip targetClip = isMenuScene ? menuMusic : gameMusic;

        if (audioSource.clip != targetClip || !audioSource.isPlaying)
        {
            // Detener fade anterior si existe
            if (activeFadeCoroutine != null)
            {
                StopCoroutine(activeFadeCoroutine);
            }
            activeFadeCoroutine = StartCoroutine(FadeSwitch(targetClip));
        }
    }

    // ======== Corrutina: Transición con fade entre pistas
    private IEnumerator FadeSwitch(AudioClip newClip)
    {
        // Fade Out
        float startVolume = audioSource.volume;
        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();

        // Fade In
        while (audioSource.volume < volume)
        {
            audioSource.volume += volume * Time.deltaTime / fadeDuration;
            yield return null;
        }

        audioSource.volume = volume; // Asegurar volumen exacto
        activeFadeCoroutine = null;
    }

    // ======== Método público: Cambia volumen dinámicamente
    public void SetVolume(float newVolume)
    {
        volume = Mathf.Clamp01(newVolume);
        // Si no hay fade activo, actualizar inmediatamente
        if (activeFadeCoroutine == null)
        {
            audioSource.volume = volume;
        }
    }

    // -------- Limpieza al destruir
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}