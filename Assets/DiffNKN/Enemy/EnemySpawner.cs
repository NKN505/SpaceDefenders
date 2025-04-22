using System.Collections;
using UnityEngine;

[System.Serializable]
public class SpawnArea
{
    public Vector3 position = Vector3.zero;                      // Posición relativa al EnemySpawner
    public Vector3 size = new Vector3(2f, 2f, 0.5f);             // Tamaño por defecto
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Config")]
    public GameObject enemyPrefab;

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

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            Vector3 spawnPos = GetValidRandomPosition();
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private Vector3 GetValidRandomPosition()
    {
        SpawnArea selectedArea = spawnAreas[Random.Range(0, spawnAreas.Length)];

        // Convertir posición local a global
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

    private void OnDrawGizmosSelected()
    {
        if (spawnAreas == null) return;

        for (int i = 0; i < spawnAreas.Length; i++)
        {
            var area = spawnAreas[i];
            Vector3 worldPos = transform.TransformPoint(area.position);

            // Dibuja el cubo del área
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(worldPos, area.size);

            // Dibuja el punto central
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(worldPos, 0.2f);

#if UNITY_EDITOR
            // Dibuja el número del área encima como texto (Editor only)
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(worldPos + Vector3.up * 0.6f, $"Zona {i + 1}");
#endif
        }
    }
}
