using UnityEngine;
using TMPro;  // Importa el espacio de nombres de TextMesh Pro

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Salud inicial del jugador
    public TextMeshProUGUI healthText;  // Referencia al texto de TMP que mostrará la salud

    private void Start()
    {
        // Inicializar el texto con la salud actual del jugador
        UpdateHealthText();
    }

    public void TakeDamage(float damage)
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
    }

    private void UpdateHealthText()
    {
        // Actualiza el texto con la salud actual (como un número)
        healthText.text = "+ " + Mathf.FloorToInt(health).ToString() ;
    }
}
