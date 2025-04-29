using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRGun : MonoBehaviour
{
    [Header("Disparo")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float shootForce = 150f; // ⚠ Fuerza reducida para evitar atravesar colliders

    [Header("Sonido")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Input")]
    public InputActionProperty triggerAction;

    [Header("Estado del Jugador")]
    public bool isDead = false;  // Flag para saber si el jugador está muerto

    [Header("Interacción con UI (Raycast)")]
    public XRRayInteractor rayInteractor;  // Ray Interactor para interactuar con la UI
    public LayerMask uiLayerMask;  // LayerMask para la UI (si la tienes configurada)

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
        if (isDead)
        {
            return; //si el jugador está muerto, no podrá disparar.
        }
        float triggerValue = triggerAction.action.ReadValue<float>(); //para leer el valor del gatillo
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

        if (audioSource != null && shootSound != null)
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f); // Pitch entre 80% y 120%. Diferente tono según disparo de forma aleatoria
            audioSource.PlayOneShot(shootSound);
        }
    }

    private void HandleUIInteraction()
    {
        RaycastHit hit;
        if (Physics.Raycast(rayInteractor.transform.position, rayInteractor.transform.forward, out hit, Mathf.Infinity, uiLayerMask))
        {
            if(hit.collider.CompareTag("UI_Button"))
            {
                var button = hit.collider.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    button.onClick.Invoke(); // Invoca el evento del botón
                }
            }
        }
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
