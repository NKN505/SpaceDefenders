using UnityEngine;

public class XRPlayerCollider : MonoBehaviour
{
    private Collider playerCollider;  // Referencia al Collider

    private void Start()
    {
        // Buscar el Collider en el objeto con tag "Player"
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            playerCollider = playerObject.GetComponentInChildren<Collider>(); // Si está en un hijo
        }
    }

    private void Update()
    {
        // Asegura que el Collider se mueva con el XR Rig
        if (playerCollider != null)
        {
            // Forzar el movimiento del Collider junto al XR Rig
            playerCollider.transform.position = transform.position;
        }
    }
}
