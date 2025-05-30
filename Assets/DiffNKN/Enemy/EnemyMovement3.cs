using UnityEngine;

public class EnemyMovement3 : MonoBehaviour
{
    public float speed = 4f;  // Velocidad más lenta
    private Transform player;
    private bool hasDamaged = false;

    public float impactHeightOffset = 0.2f; // Impacta más cerca del centro del cuerpo

    private void Start()
    {
        if (Camera.main != null)
            player = Camera.main.transform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Movimiento hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Rotación para mirar al jugador (sin inclinación vertical)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }
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
                int damageAmount = Random.Range(10, 16); // Daño aleatorio entre 10 y 15

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

                Debug.Log($"EnemyMovement3 - Daño aplicado: {damageAmount}");
            }

            Destroy(gameObject);
        }
    }
}
