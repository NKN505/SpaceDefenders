using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 10f;
    private Transform player;
    private bool hasDamaged = false; // Bandera para controlar daño único

    private void Start()
    {
        player = Camera.main.transform; // Asignar la cámara del jugador (XR Origin)
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
        // Verificar: 1) Que sea el jugador, 2) Que no haya aplicado daño ya
        if (other.CompareTag("Player") && !hasDamaged)
        {
            hasDamaged = true; // Bloquear más daños
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                int damageAmount = Random.Range(1, 6); // Daño entre 1-5
                Vector3 hitDirection = (player.position - transform.position).normalized;
                playerHealth.TakeDamage(damageAmount, hitDirection);
                Debug.Log($"Daño aplicado: {damageAmount}"); // Para debug
            }

            Destroy(gameObject); // Destruir enemigo tras el daño
        }
    }
}