using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float lifeTime = 7f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Se destruye tras 7 segundos si no impacta nada
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
        }

        Destroy(gameObject); // Se destruye al impactar cualquier cosa
    }
}
