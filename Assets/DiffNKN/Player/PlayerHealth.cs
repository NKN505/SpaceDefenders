using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Salud")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI")]
    public TextMeshProUGUI healthText;
    public DamageDirectionUI damageUI;
    public Image deathScreen;

    [Header("Audio")]
    public AudioClip damageSound;
    [Range(0f, 1f)] public float damageVolume = 1f;

    public AudioClip deathSound;
    [Range(0f, 10f)] public float deathVolume = 1f;

    public VRGun[] vrGuns;

    private AudioSource audioSource;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);

        if (damageUI != null)
            damageUI.DisableIndicators();

        // Agrega AudioSource si no existe
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.spatialBlend = 0f; // 2D sound
    }

    public void TakeDamage(float amount, Vector3 hitDirection)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Reproduce sonido de daño si aún queda salud
        if (damageSound != null && currentHealth > 0)
            audioSource.PlayOneShot(damageSound, damageVolume);

        // Muestra dirección del daño
        if (damageUI != null && hitDirection != Vector3.zero)
            damageUI.ShowDirection(hitDirection, transform, true);

        if (currentHealth <= 0)
        {
            Debug.Log("¡Has muerto!");

            if (deathSound != null)
                audioSource.PlayOneShot(deathSound, deathVolume);

            if (deathScreen != null)
                deathScreen.gameObject.SetActive(true);

            if (damageUI != null)
                damageUI.DisableIndicators();

            if (healthText != null)
                healthText.gameObject.SetActive(false);

            PlayerArmor armor = GetComponent<PlayerArmor>();
            if (armor != null)
                armor.HideArmorUI();

            Time.timeScale = 0f;

            foreach (VRGun gun in vrGuns)
            {
                if (gun != null)
                    gun.isDead = true;
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "  +" + Mathf.CeilToInt(currentHealth).ToString();
            healthText.color = (currentHealth <= 20) ? Color.red : Color.green;
        }
    }
}
