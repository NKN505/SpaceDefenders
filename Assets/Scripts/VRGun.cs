using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class XRWeapon : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireForce = 20f;
    [SerializeField] private float fireRate = 0.01f;
    [SerializeField] private AudioClip fireSound;

    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;
    private float lastFireTime;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        // Suscribirse al evento de activación (gatillo)
        grabInteractable.activated.AddListener(TriggerPulled);
    }

    private void OnDisable()
    {
        // Desuscribirse del evento para evitar memory leaks
        grabInteractable.activated.RemoveListener(TriggerPulled);
    }

    private void TriggerPulled(ActivateEventArgs arg)
    {
        // Verificar el rate of fire
        if (Time.time - lastFireTime < fireRate) return;

        Fire();
        lastFireTime = Time.time;
    }

    private void Fire()
    {
        if (projectilePrefab == null || muzzle == null)
        {
            Debug.LogWarning("Projectile prefab or muzzle not assigned!");
            return;
        }

        // Instanciar el proyectil
        GameObject projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);

        // Aplicar fuerza al proyectil
        Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
        if (projectileRb != null)
        {
            projectileRb.AddForce(muzzle.forward * fireForce, ForceMode.Impulse);
        }

        // Reproducir sonido de disparo
        if (fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }

        // Opcional: agregar efectos de partículas o retroceso aquí
    }
}