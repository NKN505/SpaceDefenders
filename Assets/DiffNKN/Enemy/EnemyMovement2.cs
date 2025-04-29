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
            // Movimiento hacia el jugador
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // Orientación hacia el jugador (sin inclinarse verticalmente)
            Vector3 direction = (player.position - transform.position).normalized;
            direction.y = 0f; // Evita que el enemigo se incline hacia arriba o abajo

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
                int damageAmount = Random.Range(6, 11); // Daño aleatorio entre 6 y 10

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
