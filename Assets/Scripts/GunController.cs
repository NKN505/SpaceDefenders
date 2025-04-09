using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GunController : MonoBehaviour
{
    public GameObject laserPrefab;
    public Transform gunTip;
    public AudioClip shootSound;
    public float laserSpeed = 20f;
    public float laserLifeTime = 2f;

    private XRBaseController xrController;
    private AudioSource audioSource;
    private bool isVRController = true;

    void Start()
    {
        xrController = GetComponentInParent<XRBaseController>();
        audioSource = GetComponent<AudioSource>();

        // Verificar si es VR o teclado
        if (xrController == null)
        {
            isVRController = false;
            Debug.Log("Configurado para teclado - VR no detectado");
        }
    }

    void Update()
    {
        // Control por teclado para pruebas
        if (!isVRController)
        {
            if ((this.gameObject.name.Contains("Left") && Input.GetKeyDown(KeyCode.Q)) ||
                (this.gameObject.name.Contains("Right") && Input.GetKeyDown(KeyCode.E)))
            {
                Shoot();
            }
        }
    }

    public void Shoot()
    {
        // Instanciar el l�ser
        GameObject laser = Instantiate(laserPrefab, gunTip.position, gunTip.rotation);

        // Configurar f�sica del l�ser
        Rigidbody rb = laser.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = gunTip.forward * laserSpeed;
        }

        // Destruir despu�s de un tiempo
        Destroy(laser, laserLifeTime);

        // Reproducir sonido
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Vibraci�n para VR
        if (isVRController && xrController != null)
        {
            xrController.SendHapticImpulse(0.5f, 0.1f);
        }
    }
}