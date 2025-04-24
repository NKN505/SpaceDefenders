using UnityEngine;
using System.Collections;

[System.Serializable]
public class SpawnArea
{
    public Vector3 position = Vector3.zero;
    public Vector3 size = new Vector3(2f, 2f, 0.5f);
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs (con probabilidades)")]
    public GameObject enemyPrefab1; // 50%
    public GameObject enemyPrefab2; // 30%
    public GameObject enemyPrefab3; // 20%

    [Header("Spawn Areas")]
    public SpawnArea[] spawnAreas;

    [Header("Spawn Timing")]
    public float minSpawnInterval = 1f;
    public float maxSpawnInterval = 5f;

    [Header("Spawn Restrictions")]
    public float minSpawnDistance = 2f;
    public float spawnCheckRadius = 1f;
    public LayerMask obstacleMask;

    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;
    private ScoreCount scoreCount;

    private void Start()
    {
        // Obtén la referencia a ScoreCount
        scoreCount = Object.FindFirstObjectByType<ScoreCount>();

        if (scoreCount == null)
        {
            Debug.LogError("No se encontró ScoreCount en la escena.");
        }

        // Iniciar el proceso de aparición de enemigos
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector3 spawnPos = GetValidRandomPosition();

            GameObject enemyToSpawn = GetRandomEnemyPrefab();
            if (enemyToSpawn != null)
            {
                GameObject enemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    // Establecer el puntaje del enemigo dependiendo del prefab
                    if (enemyToSpawn == enemyPrefab1)
                        enemyHealth.enemyScore = 25;
                    else if (enemyToSpawn == enemyPrefab2)
                        enemyHealth.enemyScore = 75;
                    else if (enemyToSpawn == enemyPrefab3)
                        enemyHealth.enemyScore = 120;

                    // Suscribirse al evento onDeath
                    enemyHealth.onDeath += OnEnemyDeath;
                }
            }

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float roll = Random.Range(0f, 1f);

        if (roll < 0.5f)       // 50%
            return enemyPrefab1;
        else if (roll < 0.8f)  // 30%
            return enemyPrefab2;
        else                   // 20%
            return enemyPrefab3;
    }

    private Vector3 GetValidRandomPosition()
    {
        SpawnArea selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
        Vector3 center = transform.TransformPoint(selectedArea.position);
        Vector3 randomPos = Vector3.zero;
        int attempts = 0;

        do
        {
            float x = Random.Range(-selectedArea.size.x / 2, selectedArea.size.x / 2);
            float y = Random.Range(-selectedArea.size.y / 2, selectedArea.size.y / 2);
            float z = Random.Range(-selectedArea.size.z / 2, selectedArea.size.z / 2);

            randomPos = center + new Vector3(x, y, z);
            attempts++;

            if (attempts > 20) break;

        } while (
            Vector3.Distance(randomPos, lastSpawnPosition) < minSpawnDistance ||
            Physics.CheckSphere(randomPos, spawnCheckRadius, obstacleMask)
        );

        lastSpawnPosition = randomPos;
        return randomPos;
    }

    private void OnEnemyDeath(EnemyHealth enemyHealth)
    {
        if (scoreCount != null)
        {
            // Obtener la distancia desde el enemigo hasta el punto de muerte
            float distance = Vector3.Distance(enemyHealth.transform.position, Camera.main.transform.position);

            // Puntaje base del enemigo
            int score = enemyHealth.enemyScore;

            // Sumar bonus de distancia
            score += Mathf.CeilToInt(distance);

            // Actualizar el puntaje
            scoreCount.AddScore(score);

            Debug.Log($"Puntaje del enemigo: {score}");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            var area = spawnAreas[i];
            Vector3 worldPos = transform.TransformPoint(area.position);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, area.size);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldPos, 0.2f);

#if UNITY_EDITOR
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(worldPos + Vector3.up * 0.6f, $"Zona {i + 1}");
#endif
        }
    }
}
