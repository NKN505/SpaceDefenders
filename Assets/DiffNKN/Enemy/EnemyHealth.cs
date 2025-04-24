using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Salud")]
    public int maxHealth = 40;
    private int currentHealth;

    // Evento que notifica la muerte del enemigo
    public event Action<EnemyHealth> onDeath;

    // Puntuación del enemigo
    public int enemyScore = 0;

    private bool isDead = false;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void OnEnable()
    {
        isDead = false;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        Debug.Log($"{gameObject.name} ha muerto. Disparando evento onDeath.");
        onDeath?.Invoke(this); // Llamamos al evento onDeath
        Destroy(gameObject); // Destruimos el objeto
    }

    private void OnDestroy()
    {
        onDeath = null; // Aseguramos que el evento se limpie cuando el objeto se destruya
    }
}
