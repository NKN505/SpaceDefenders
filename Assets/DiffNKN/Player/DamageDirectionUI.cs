using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageDirectionUI : MonoBehaviour
{
    public Image upIndicator;    // Frente
    public Image downIndicator;  // Atrás
    public Image leftIndicator;
    public Image rightIndicator;
    public float showTime = 0.5f;
    public float fadeDuration = 0.3f;
    public float maxAlpha = 0.6f; // Nueva opacidad máxima

    private void Start()
    {
        DisableIndicators(); // Desactiva los indicadores al inicio
    }

    public void ShowDirection(Vector3 hitDirection, Transform playerTransform)
    {
        Debug.Log("ShowDirection llamada con hitDirection: " + hitDirection);

        Vector3 localDir = playerTransform.InverseTransformDirection(hitDirection);

        if (Mathf.Abs(localDir.z) > Mathf.Abs(localDir.x))
        {
            if (localDir.z < 0)
                StartCoroutine(FadeIndicator(upIndicator));   // Golpe desde delante
            else
                StartCoroutine(FadeIndicator(downIndicator)); // Golpe desde detrás
        }
        else
        {
            if (localDir.x < 0)
                StartCoroutine(FadeIndicator(leftIndicator)); // Golpe desde izquierda
            else
                StartCoroutine(FadeIndicator(rightIndicator)); // Golpe desde derecha
        }
    }

    private IEnumerator FadeIndicator(Image indicator)
    {
        if (indicator == null) yield break;

        Color originalColor = indicator.color;
        originalColor.a = 0f;
        indicator.color = originalColor;
        indicator.enabled = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, maxAlpha, t / fadeDuration); // Aquí limitamos el alpha
            indicator.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        yield return new WaitForSeconds(showTime);

        t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(maxAlpha, 0f, t / fadeDuration); // Igual aquí
            indicator.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        indicator.enabled = false;
    }

    public void DisableIndicators()
    {
        // Desactiva los indicadores cuando el jugador muere
        if (upIndicator != null) upIndicator.enabled = false;
        if (downIndicator != null) downIndicator.enabled = false;
        if (leftIndicator != null) leftIndicator.enabled = false;
        if (rightIndicator != null) rightIndicator.enabled = false;
    }
}
