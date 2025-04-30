using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    // La posición de respawn donde el jugador será reubicado.
    public Transform respawnPoint;

    // Este método se llama cuando otro collider entra en el trigger
    private void OnTriggerEnter(Collider other)
    {
        // Verificar si el objeto que entra es el jugador
        if (other.CompareTag("Player"))
        {
            // Reubicar al jugador en el punto de respawn
            other.transform.position = respawnPoint.position;

            // Si deseas agregar alguna animación o efecto cuando el jugador reaparece, puedes hacerlo aquí
            // Ejemplo: other.GetComponent<Animator>().SetTrigger("Respawn");
        }
    }
}
