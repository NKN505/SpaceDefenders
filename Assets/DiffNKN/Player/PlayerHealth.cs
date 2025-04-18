using UnityEngine;
using TMPro; // Asegúrate de tener esto para usar TextMeshProUGUI
using UnityEngine.UI; // Necesario para usar Image

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public TextMeshProUGUI healthText;
    public DamageDirectionUI damageUI;

    public Image deathScreen; // Imagen que se activa al morir

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (deathScreen != null)
        {
            deathScreen.gameObject.SetActive(false); // Oculta al iniciar
        }

        if (damageUI != null)
        {
            damageUI.DisableIndicators(); // Desactiva los indicadores al iniciar
        }
    }

    public void TakeDamage(float amount, Vector3 hitDirection)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (damageUI != null)
        {
            damageUI.ShowDirection(hitDirection, transform);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("¡Jugador muerto!");
            if (deathScreen != null)
            {
                deathScreen.gameObject.SetActive(true); // Muestra pantalla de muerte
            }

            if (damageUI != null)
            {
                damageUI.DisableIndicators(); // Desactiva los indicadores al morir
            }
        }
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "+ " + Mathf.CeilToInt(currentHealth).ToString();

            if (currentHealth <= 20)
            {
                healthText.color = Color.red;
            }
            else
            {
                healthText.color = Color.green;
            }
        }
    }
}
