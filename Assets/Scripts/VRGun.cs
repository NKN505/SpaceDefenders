using UnityEngine;
using UnityEngine.InputSystem;

public class VRGun : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float shootForce = 150f; // ⚠ Fuerza reducida para evitar atravesar colliders

    [Header("Input")]
    public InputActionProperty triggerAction;

    private void OnEnable()
    {
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        float triggerValue = triggerAction.action.ReadValue<float>();
        Debug.Log("Trigger Value: " + triggerValue);

        if (triggerAction.action.WasPressedThisFrame())
        {
            Debug.Log("Trigger PRESSED");
            Shoot();
        }
    }

    void Shoot()
    {
        // Ajusta la rotación del proyectil sumando 90 grados en el eje X
        Quaternion adjustedRotation = spawnPoint.rotation * Quaternion.Euler(90f, 0f, 0f);

        // Instancia el proyectil con la rotación ajustada
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, adjustedRotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Habilita detección de colisiones precisa
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            // Aplica fuerza reducida
            rb.AddForce(spawnPoint.forward * shootForce, ForceMode.Impulse);
        }

        Debug.Log("Shooting");
    }

    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + spawnPoint.forward * 0.5f);
        }
    }
}
