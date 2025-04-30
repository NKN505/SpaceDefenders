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
        pickupCollider.enabled = false;
        pickupRenderer.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        pickupCollider.enabled = true;
        pickupRenderer.enabled = true;
    }

    // 🟠 Gizmo visible solo cuando el objeto está seleccionado
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.2f, 0.8f, 1f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, 1f); // valor fijo ya que el rango sonoro se eliminó
    }
}
