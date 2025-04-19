using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageDirectionUI : MonoBehaviour
{
    public Image upIndicator;
    public Image downIndicator;
    public Image leftIndicator;
    public Image rightIndicator;

    public float showTime = 0.5f;
    public float fadeDuration = 0.3f;
    public float maxAlpha = 0.6f;

    private void Start()
    {
        DisableIndicators();
    }

    /// <summary>
    /// Muestra la flecha correspondiente. color: rojo si daño a salud, amarillo si solo armadura.
    /// </summary>
    public void ShowDirection(Vector3 hitDirection, Transform playerTransform, bool healthHit = true)
    {
        DisableIndicators();
        
        // Calcula la dirección en el plano horizontal
        Vector3 dir = new Vector3(hitDirection.x, 0f, hitDirection.z).normalized;
        float angle = Vector3.SignedAngle(playerTransform.forward, dir, Vector3.up);

        Image indicator = null;
        if (angle > -45f && angle <= 45f)
            indicator = upIndicator;
        else if (angle > 45f && angle <= 135f)
            indicator = rightIndicator;
        else if (angle > 135f || angle <= -135f)
            indicator = downIndicator;
        else
            indicator = leftIndicator;

        if (indicator != null)
        {
            // Ajusta color según tipo de daño
            indicator.color = healthHit ? 
                new Color(1f, 0f, 0f, indicator.color.a) :  // Rojo
                new Color(1f, 1f, 0f, indicator.color.a);   // Amarillo
            StartCoroutine(FadeIndicator(indicator));
        }
    }

    private IEnumerator FadeIndicator(Image indicator)
    {
        if (indicator == null) yield break;

        Color original = indicator.color;
        indicator.color = new Color(original.r, original.g, original.b, 0f);
        indicator.enabled = true;

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(0f, maxAlpha, t / fadeDuration);
            indicator.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        yield return new WaitForSeconds(showTime);
        t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(maxAlpha, 0f, t / fadeDuration);
            indicator.color = new Color(original.r, original.g, original.b, a);
            yield return null;
        }

        indicator.enabled = false;
    }

    public void DisableIndicators()
    {
        if (upIndicator != null) upIndicator.enabled = false;
        if (downIndicator != null) downIndicator.enabled = false;
        if (leftIndicator != null) leftIndicator.enabled = false;
        if (rightIndicator != null) rightIndicator.enabled = false;
    }
}
