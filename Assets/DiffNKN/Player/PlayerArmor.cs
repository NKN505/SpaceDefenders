using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class PlayerArmor : MonoBehaviour
{
    [Header("Armor Stats")]
    public float armor = 0f;
    public float maxArmor = 100f;

    [Header("UI References")]
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI armorSymbol;
    public Canvas armorCanvas;

    [Header("Audio")]
    public AudioClip armorPickupSound;
    public float armorPickupVolume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        armor = Mathf.Clamp(armor, 0, maxArmor);
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0f; // Sonido 2D
        UpdateArmorUI();
    }

    public void AbsorbDamage(float damageAmount, PlayerHealth playerHealth, Vector3 hitDirection)
    {
        if (playerHealth == null) return;

        int damage = Mathf.CeilToInt(damageAmount);

        if (armor >= damage && Random.value <= 0.2f)
        {
            armor -= damage;
            UpdateArmorUI();
            Debug.Log($"Armadura absorbió el 100% del daño: {damage}");

            playerHealth?.damageUI?.ShowDirection(hitDirection, playerHealth.transform, false);
            return;
        }

        if (armor > 0)
        {
            int half = Mathf.CeilToInt(damage * 0.5f);
            int absorbed = Mathf.Min(half, Mathf.FloorToInt(armor));
            armor -= absorbed;

            UpdateArmorUI();
            Debug.Log($"Armadura absorbió {absorbed} de daño.");

            int remaining = damage - absorbed;
            if (remaining > 0)
                playerHealth.TakeDamage(remaining, hitDirection);
            else
                playerHealth?.damageUI?.ShowDirection(hitDirection, playerHealth.transform, false);
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

        if (armorPickupSound != null)
            audioSource.PlayOneShot(armorPickupSound, armorPickupVolume);

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

        if (armorCanvas != null)
            armorCanvas.gameObject.SetActive(true);
    }

    public void HideArmorUI()
    {
        if (armorCanvas != null)
            armorCanvas.gameObject.SetActive(false);

        if (armorText != null)
            armorText.gameObject.SetActive(false);

        if (armorSymbol != null)
            armorSymbol.gameObject.SetActive(false);
    }
}
