using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 40;
    private int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"{gameObject.name} recibió {amount} de daño. Salud restante: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} has muerto.");
        Destroy(gameObject);
    }
}
