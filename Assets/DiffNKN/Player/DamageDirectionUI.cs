using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageDirectionUI : MonoBehaviour
{
    public Image upIndicator;
    public Image downIndicator;
    public Image leftIndicator;
    public Image rightIndicator;
    public float showTime = 0.5f; // Duración del indicador

    public void ShowDirection(Vector3 hitDirection, Transform playerTransform)
    {
        // Convertimos la dirección del golpe al espacio local del jugador
        Vector3 localDir = playerTransform.InverseTransformDirection(hitDirection);

        // Determinamos cuál dirección activar
        if (Mathf.Abs(localDir.z) > Mathf.Abs(localDir.x))
        {
            // Golpe más fuerte en el eje Z
            if (localDir.z > 0)
                StartCoroutine(FlashIndicator(downIndicator)); // Golpe desde delante
            else
                StartCoroutine(FlashIndicator(upIndicator));   // Golpe desde detrás
        }
        else if (Mathf.Abs(localDir.x) > Mathf.Abs(localDir.z))
        {
            // Golpe más fuerte en el eje X
            if (localDir.x > 0)
                StartCoroutine(FlashIndicator(leftIndicator)); // Golpe desde la derecha
            else
                StartCoroutine(FlashIndicator(rightIndicator)); // Golpe desde la izquierda
        }
        else
        {
            // Golpe diagonal: activamos ambos indicadores, dependiendo de la dirección
            if (localDir.x > 0 && localDir.z > 0)
            {
                StartCoroutine(FlashIndicator(leftIndicator));  // Golpe desde abajo a la derecha
                StartCoroutine(FlashIndicator(downIndicator)); // Golpe desde abajo a la derecha
            }
            else if (localDir.x < 0 && localDir.z > 0)
            {
                StartCoroutine(FlashIndicator(rightIndicator)); // Golpe desde abajo a la izquierda
                StartCoroutine(FlashIndicator(downIndicator));  // Golpe desde abajo a la izquierda
            }
            else if (localDir.x > 0 && localDir.z < 0)
            {
                StartCoroutine(FlashIndicator(leftIndicator));  // Golpe desde arriba a la derecha
                StartCoroutine(FlashIndicator(upIndicator));   // Golpe desde arriba a la derecha
            }
            else
            {
                StartCoroutine(FlashIndicator(rightIndicator)); // Golpe desde arriba a la izquierda
                StartCoroutine(FlashIndicator(upIndicator));   // Golpe desde arriba a la izquierda
            }
        }
    }

    private IEnumerator FlashIndicator(Image indicator)
    {
        indicator.enabled = true;
        yield return new WaitForSeconds(showTime);
        indicator.enabled = false;
    }
}
