using UnityEngine;
using TMPro;  // Importa el espacio de nombres de TextMesh Pro

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Salud inicial del jugador
    public TextMeshProUGUI healthText;  // Referencia al texto de TMP que mostrará la salud
    public DamageDirectionUI damageUI;  // Referencia al script de UI para la dirección del daño

    private void Start()
    {
        // Inicializar el texto con la salud actual del jugador
        UpdateHealthText();
    }

    public void TakeDamage(float damage, Vector3 hitDirection)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            // Aquí puedes manejar lo que sucede cuando el jugador muere
            Debug.Log("¡El jugador ha muerto!");
        }

        // Actualiza el texto de la salud
        UpdateHealthText();

        // Mostrar dirección del golpe
        if (damageUI != null)
        {
            damageUI.ShowDirection(hitDirection, transform);  // Llamamos a ShowDirection de DamageDirectionUI
        }
    }

    private void UpdateHealthText()
    {
        // Convierte salud a entero y actualiza el texto
        int currentHealth = Mathf.FloorToInt(health);
        healthText.text = "+ " + currentHealth.ToString();

        // Cambia el color dependiendo de la salud
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
