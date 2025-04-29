using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    private Transform player;
    private bool hasDamaged = false;

    // Altura de impacto respecto al centro del jugador
    public float impactHeightOffset = 0.2f;

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

            // Orientación del enemigo hacia el jugador (sin cambiar el eje Y del enemigo)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f; // Para evitar inclinación vertical
            if (direction != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // Rotación suave
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
                int damageAmount = Random.Range(1, 6); // Daño aleatorio 1-5

                // Altura ajustada para el impacto
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

                Debug.Log($"Daño aplicado: {damageAmount}");
            }

            Destroy(gameObject);
        }
    }
}
