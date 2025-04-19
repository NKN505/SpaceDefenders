using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerArmor : MonoBehaviour
{
    [Header("Armor Stats")]
    public float armor = 0f;
    public float maxArmor = 100f;

    [Header("UI References")]
    public TextMeshProUGUI armorText;    // Texto numérico de la armadura
    public TextMeshProUGUI armorSymbol;  // Símbolo de armadura
    public Canvas armorCanvas;           // Canvas completo de la UI de armadura

    private void Start()
    {
        armor = Mathf.Clamp(armor, 0, maxArmor);
        UpdateArmorUI();
    }

    /// <summary>
    /// Absorbe parte del daño y pasa el resto a PlayerHealth.
    /// Si solo la armadura reduce el daño (salud intacta), activa indicador amarillo.
    /// </summary>
    public void AbsorbDamage(float damageAmount, PlayerHealth playerHealth, Vector3 hitDirection)
    {
        if (playerHealth == null) return;

        if (armor > 0)
        {
            int half = Mathf.CeilToInt(damageAmount * 0.5f);
            float damageToAbsorb = Mathf.Min(half, armor);

            armor -= damageToAbsorb;
            UpdateArmorUI();
            Debug.Log($"Armadura absorbió {damageToAbsorb} de daño.");

            float remainingDamage = damageAmount - damageToAbsorb;
            if (remainingDamage > 0)
            {
                // Daño restante a salud (rojo)
                playerHealth.TakeDamage(remainingDamage, hitDirection);
            }
            else
            {
                // Solo armadura (amarillo)
                if (playerHealth.damageUI != null)
                    playerHealth.damageUI.ShowDirection(hitDirection, playerHealth.transform, false);
            }
        }
        else
        {
            // Sin armadura, daño directo (rojo)
            playerHealth.TakeDamage(damageAmount, hitDirection);
        }
    }

    public void AddArmor(float amount)
    {
        armor += amount;
        armor = Mathf.Clamp(armor, 0, maxArmor);
        Debug.Log($"Armadura obtenida. Valor actual: {armor}");
        UpdateArmorUI();
    }

    private void UpdateArmorUI()
    {
        int displayArmor = Mathf.CeilToInt(armor);

        if (armorText != null)
        {
            armorText.text = displayArmor.ToString();
            armorText.color = (armor <= 20) ? Color.yellow : Color.cyan;
            armorText.gameObject.SetActive(true);
        }

        if (armorSymbol != null)
        {
            armorSymbol.text = "D";
            armorSymbol.color = (armor <= 20) ? Color.yellow : Color.cyan;
            armorSymbol.gameObject.SetActive(true);
        }

        if (armorCanvas != null && !armorCanvas.gameObject.activeSelf)
            armorCanvas.gameObject.SetActive(true);
    }

    /// <summary>
    /// Oculta la UI completa de armadura cuando el jugador muere.
    /// </summary>
    public void HideArmorUI()
    {
        if (armorCanvas != null)
            armorCanvas.gameObject.SetActive(false);
        else
        {
            if (armorText != null)
                armorText.gameObject.SetActive(false);
            if (armorSymbol != null)
                armorSymbol.gameObject.SetActive(false);
        }
    }
}