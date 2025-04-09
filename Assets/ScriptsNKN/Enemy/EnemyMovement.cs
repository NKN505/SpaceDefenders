using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
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
            // Aquí destruyes el enemigo después de la colisión
            Destroy(gameObject);  // Destruye al enemigo que colisionó con la cámara
        }
    }
}
