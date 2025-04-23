using UnityEngine;
using UnityEngine.InputSystem;

public class VRGun : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float shootForce = 500f;

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
        Quaternion adjustedRotation = spawnPoint.rotation * Quaternion.Euler(90f, 0f, 0f);
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, adjustedRotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * shootForce);
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
