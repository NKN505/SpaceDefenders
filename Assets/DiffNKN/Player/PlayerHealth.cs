using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public TextMeshProUGUI healthText;   // Texto que muestra la salud
    public DamageDirectionUI damageUI;   // UI para la dirección del daño
    public Image deathScreen;            // Imagen de la pantalla de muerte
    public VRGun[] vrGuns;                  // Referencia al script VRGun de las pistolas

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreen != null)
            deathScreen.gameObject.SetActive(false);

        if (damageUI != null)
            damageUI.DisableIndicators();

    }

    public void TakeDamage(float amount, Vector3 hitDirection)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHealthUI();

        // Muestra siempre indicador de salud (rojo)
        if (damageUI != null && hitDirection != Vector3.zero)
            damageUI.ShowDirection(hitDirection, transform, true);

        if (currentHealth <= 0)
        {
            Debug.Log("¡Has muerto!");
            if (deathScreen != null)
                deathScreen.gameObject.SetActive(true);
            if (damageUI != null)
                damageUI.DisableIndicators();
            if (healthText != null)
                healthText.gameObject.SetActive(false);

            PlayerArmor armor = GetComponent<PlayerArmor>();
            if (armor != null)
                armor.HideArmorUI();

            Time.timeScale = 0f; //pausamos el juego por completo

            foreach (VRGun gun in vrGuns)
            {
                if (gun != null)
                {
                    gun.isDead = true;  // Deja de disparar en ambas pistolas
                }
            }

            // Espera 5 segundos y después vuelves a la escena principal

            StartCoroutine(ReinicioPausa(5f));

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

    private IEnumerator ReinicioPausa(float delay)
    {
        // Esperamos en tiempo real (ya que Time.timeScale = 0f)
        yield return new WaitForSecondsRealtime(delay);

        Time.timeScale = 1f; // Restauramos el tiempo
        SceneManager.LoadScene("Portada"); // Cambia "MainMenu" por el nombre real de tu escena principal
    }
}
