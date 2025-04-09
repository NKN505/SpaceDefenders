using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;  // Salud inicial del jugador

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // Aquí puedes manejar lo que sucede cuando el jugador muere
            Debug.Log("¡El jugador ha muerto!");
        }
    }
}
