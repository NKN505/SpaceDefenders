using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerArmor : MonoBehaviour
{
    [Header("Armor Stats")]
    public float armor = 0f;
    public float maxArmor = 100f;

    [Header("UI References")]
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI armorSymbol;
    public Canvas armorCanvas;

    private void Start()
    {
        armor = Mathf.Clamp(armor, 0, maxArmor);
        UpdateArmorUI();
    }

    public void AbsorbDamage(float damageAmount, PlayerHealth playerHealth, Vector3 hitDirection)
    {
        if (playerHealth == null) return;

        int damage = Mathf.CeilToInt(damageAmount); // Sin decimales

        // 20% de probabilidad de absorber todo el daño
        if (Random.value <= 0.2f && armor >= damage)
        {
            armor -= damage;
            UpdateArmorUI();
            Debug.Log($"Armadura absorbió el 100% del daño: {damage} puntos.");

            // Mostrar feedback amarillo
            if (playerHealth.damageUI != null)
                playerHealth.damageUI.ShowDirection(hitDirection, playerHealth.transform, false);

            return;
        }

        if (armor > 0)
        {
            int half = Mathf.CeilToInt(damage * 0.5f);
            int damageToAbsorb = Mathf.Min(half, Mathf.FloorToInt(armor)); // Sin decimales

            armor -= damageToAbsorb;
            UpdateArmorUI();
            Debug.Log($"Armadura absorbió {damageToAbsorb} de daño.");

            int remainingDamage = damage - damageToAbsorb;
            if (remainingDamage > 0)
            {
                playerHealth.TakeDamage(remainingDamage, hitDirection);
            }
            else
            {
                if (playerHealth.damageUI != null)
                    playerHealth.damageUI.ShowDirection(hitDirection, playerHealth.transform, false);
            }
        }
        else
        {
            playerHealth.TakeDamage(damage, hitDirection);
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
