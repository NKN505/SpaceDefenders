using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // Prefab del enemigo que quieres que se genere
    public Transform spawnPoint;    // Único punto de spawn
    public float spawnInterval = 3f; // Intervalo de tiempo para generar enemigos

    private void Start()
    {
        // Comienza a generar enemigos a intervalos regulares
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true) // Genera enemigos de manera continua
        {
            // Instancia un enemigo en el único punto de spawn
            Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

            // Espera un intervalo antes de generar el siguiente enemigo
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
