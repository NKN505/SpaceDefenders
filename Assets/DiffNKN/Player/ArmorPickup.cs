using UnityEngine;
using System.Collections;

public class ArmaduraPickup : MonoBehaviour
{
    public float armorAmount = 50f;
    public float respawnTime = 90f;

    private Collider pickupCollider;
    private Renderer pickupRenderer;

    private void Start()
    {
        // Cacheamos referencias al inicio
        pickupCollider = GetComponent<Collider>();
        pickupRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerArmor playerArmor = other.GetComponent<PlayerArmor>();

        if (playerArmor != null)
        {
            playerArmor.AddArmor(armorAmount);
            StartCoroutine(Respawn());
        }
    }

    private IEnumerator Respawn()
    {
        // Ocultamos el objeto
        pickupCollider.enabled = false;
        pickupRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        // Volvemos a activar el objeto
        pickupCollider.enabled = true;
        pickupRenderer.enabled = true;
    }
}
