using UnityEngine;

public class EnemyMovement2 : MonoBehaviour
{
    public float speed = 7f;  // Velocidad reducida
    private Transform player;
    private bool hasDamaged = false;

    public float impactHeightOffset = 0.2f; // Impacto más abajo en el cuerpo

    private void Start()
    {
        if (Camera.main != null)
            player = Camera.main.transform;
    }

    private void Update()
    {
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasDamaged && other.CompareTag("Player"))
        {
            hasDamaged = true;

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            PlayerArmor playerArmor = other.GetComponent<PlayerArmor>();

            if (playerHealth != null)
            {
                int damageAmount = Random.Range(6, 11); // Daño aleatorio entre 6 y 10 (11 es exclusivo)

                Vector3 targetPosition = playerHealth.transform.position;
                targetPosition.y -= impactHeightOffset;

                Vector3 hitDirection = (targetPosition - transform.position).normalized;

                if (playerArmor != null)
                {
                    playerArmor.AbsorbDamage(damageAmount, playerHealth, hitDirection);
                }
                else
                {
                    playerHealth.TakeDamage(damageAmount, hitDirection);
                }

                Debug.Log($"EnemyMovement2 - Daño aplicado: {damageAmount}");
            }

            Destroy(gameObject);
        }
    }
}
