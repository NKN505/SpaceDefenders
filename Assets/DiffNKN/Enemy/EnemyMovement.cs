using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    private Transform player;
    private bool hasDamaged = false;

    // Define la altura a la que quieres que los enemigos impacten (por ejemplo, al nivel del pecho)
    public float impactHeightOffset = 1.2f;  // Ajusta esto según la altura del jugador

    private void Start()
    {
        if (Camera.main != null)
            player = Camera.main.transform; // Asegura que haya cámara principal
    }

    private void Update()
    {
        if (player != null)
        {
            // Movimiento hacia el jugador
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
                int damageAmount = Random.Range(1, 6); // Daño aleatorio 1-5

                // Ajusta la posición de impacto a una altura más baja en el jugador
                Vector3 targetPosition = playerHealth.transform.position;
                targetPosition.y -= impactHeightOffset;  // Baja la altura para impactar el cuerpo, no la cabeza

                // Dirección hacia la nueva posición de impacto
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

            Destroy(gameObject); // Eliminar enemigo después del impacto
        }
    }
}
