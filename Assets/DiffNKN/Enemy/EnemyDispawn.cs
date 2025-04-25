using UnityEngine;

public class EnemyDispawn : MonoBehaviour
{
    // Tag con el que identificamos a los enemigos
    [SerializeField] private string enemyTag = "Enemy";

    // Se llama automáticamente cuando otro collider entra en este trigger
    private void OnTriggerEnter(Collider other)
    {
        // Si el otro objeto tiene el tag "Enemy", lo destruimos
        if (other.CompareTag(enemyTag))
        {
            Destroy(other.gameObject);
        }
    }
}
