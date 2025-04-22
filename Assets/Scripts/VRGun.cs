using UnityEngine;
using UnityEngine.InputSystem;

public class VRGun : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float shootForce = 500f;

    [Header("Input")]
    public InputActionProperty triggerAction; // Asigna el gatillo desde el editor

    void Update()
    {
        // Si se presiona el gatillo
        if (triggerAction.action.WasPressedThisFrame())
        {
            Shoot();
        }
       
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * shootForce);
        }
    }
}
