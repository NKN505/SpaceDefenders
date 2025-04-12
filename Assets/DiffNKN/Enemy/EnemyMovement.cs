using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    private Transform player;

    private void Start()
    {
        // Obtener la referencia del jugador (que en este caso es la cámara principal del XR Origin)
        player = Camera.main.transform;  // Asumiendo que la cámara del jugador tiene la etiqueta MainCamera
    }

    private void Update()
    {
        // El enemigo se mueve hacia la cámara del jugador
        if (player != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }

    // Este método detecta cuando el enemigo toca el Collider del jugador
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // Asegúrate de que la cámara tiene la etiqueta "Player"
        {
            // Obtener el componente PlayerHealth del jugador
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                // Generar un daño aleatorio entre 1 y 5 puntos
                int damageAmount = Random.Range(1, 6);  // Obtiene un valor entre 1 y 5

                // Aplicar el daño al jugador
                playerHealth.TakeDamage(damageAmount);
            }

            // Destruir el enemigo después de hacerle daño al jugador
            Destroy(gameObject);
        }
    }
}
