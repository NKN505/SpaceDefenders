using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;

    void Start()
    {
        // Comenzar con fade-out desde negro
        if (fadeImage != null)
        {
            fadeImage.color = new Color(0f, 0f, 0f, 1f);
            StartCoroutine(FadeFromBlack());
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoadScene(sceneName));
    }

    IEnumerator FadeAndLoadScene(string sceneName)
    {
        // Fade In (hacia negro)
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        Time.timeScale = 1f; // Por si estaba en pausa
        SceneManager.LoadScene(0);
    }

    IEnumerator FadeFromBlack()
    {
        // Fade Out (desde negro)
        float t = 0f;
        Color color = fadeImage.color;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.gameObject.SetActive(false); // Ocultar el panel luego del fade
    }
}
