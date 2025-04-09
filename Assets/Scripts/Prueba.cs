using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class Prueba : MonoBehaviour
{
    [Header("Configuraci�n de Disparo")]
    [SerializeField] private Transform muzzlePoint; // Punto de origen del l�ser
    [SerializeField] private float shotSpeed = 20f; // Velocidad del l�ser
    [SerializeField] private float maxLaserDistance = 50f; // Distancia m�xima del l�ser
    [SerializeField] private float destroyLaserAfter = 3f; // Tiempo antes de destruir el l�ser si no impacta

    [Header("Referencias")]
    [SerializeField] private GameObject laserPrefab; // Prefab del l�ser
    [SerializeField] private AudioClip shotSound; // Sonido de disparo
    [SerializeField] private InputActionProperty triggerAction; // Acci�n del gatillo

    private AudioSource audioSource;
    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    private void Awake()
    {
        // Obtener referencias necesarias
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // Sonido 3D
        }

        // Configurar eventos de agarre
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnEnable()
    {
        // Habilitar la acci�n del gatillo
        triggerAction.action.Enable();
    }

    private void OnDisable()
    {
        // Deshabilitar la acci�n del gatillo
        triggerAction.action.Disable();
    }

    private void Update()
    {
        if (!isGrabbed) return;

        // Verificar si se presion� el gatillo
        float triggerValue = triggerAction.action.ReadValue<float>();

        if (triggerValue > 0.8f) // Umbral para considerar "disparo"
        {
            Shoot();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
    }

    private void Shoot()
    {
        // Instanciar el l�ser
        GameObject laser = Instantiate(laserPrefab, muzzlePoint.position, muzzlePoint.rotation);

        // Configurar el l�ser
        Rigidbody laserRb = laser.GetComponent<Rigidbody>();
        if (laserRb != null)
        {
            laserRb.linearVelocity = muzzlePoint.forward * shotSpeed;
        }

        // Configurar destrucci�n autom�tica
        Destroy(laser, destroyLaserAfter);

        // Reproducir sonido de disparo
        if (shotSound != null)
        {
            audioSource.PlayOneShot(shotSound);
        }

        // Opcional: Raycast para impacto inmediato
        RaycastHit hit;
        if (Physics.Raycast(muzzlePoint.position, muzzlePoint.forward, out hit, maxLaserDistance))
        {
            // Aqu� puedes manejar el impacto si lo necesitas
            // Por ejemplo: hit.collider.SendMessage("OnHit", SendMessageOptions.DontRequireReceiver);
        }
    }

    // Dibujar gizmo para visualizar el punto de disparo
    private void OnDrawGizmosSelected()
    {
        if (muzzlePoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(muzzlePoint.position, muzzlePoint.position + muzzlePoint.forward * 2f);
        }
    }
}