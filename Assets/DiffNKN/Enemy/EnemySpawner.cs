using UnityEngine;
using TMPro;
using System.Collections;

[System.Serializable]
public class SpawnArea
{
    public Vector3 position = Vector3.zero;
    public Vector3 size = new Vector3(2f, 2f, 0.5f);
}

public class EnemySpawner : MonoBehaviour
{
    public enum GameLevel
    {
        Nivel1,
        Nivel2,
        Nivel3,
        Maestro,
        Leyenda,
        Caos
    }

    [Header("Enemy Prefabs (con probabilidades)")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;

    [Header("Spawn Areas")]
    public SpawnArea[] spawnAreas;

    [Header("Spawn Timing")]
    public float[] minSpawnIntervals = { 4.0f, 2.8f, 1.8f, 1.0f, 0.5f, 0.1f };
    public float[] maxSpawnIntervals = { 6.0f, 4.0f, 3.0f, 1.5f, 1.0f, 0.3f };

    [Header("Spawn Restrictions")]
    public float minSpawnDistance = 2f;
    public float spawnCheckRadius = 1f;
    public LayerMask obstacleMask;

    [Header("UI de Nivel")]
    public Canvas nivelCanvas;
    public TMP_Text nivelText;
    public TMP_Text romanLevelText; // Nuevo TMP_Text para el nivel en números romanos

    private Vector3 lastSpawnPosition = Vector3.positiveInfinity;
    private ScoreCount scoreCount;
    private GameLevel currentLevel = GameLevel.Nivel1;
    private bool spawningActive = false;

    private void Start()
    {
        scoreCount = Object.FindFirstObjectByType<ScoreCount>();

        if (scoreCount == null)
        {
            Debug.LogError("No se encontró ScoreCount en la escena.");
        }

        if (nivelCanvas != null)
            nivelCanvas.enabled = false;  // Debería desactivarse solo en transiciones, no afecta romanLevelText

        if (romanLevelText != null)
        {
            romanLevelText.text = GetRomanNumeral(currentLevel);  // Mostrar el nivel en romano al inicio
        }

        StartCoroutine(GameLoop());
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator GameLoop()
    {
        // Primer nivel al inicio
        yield return StartCoroutine(HandleLevelTransition(currentLevel));

        // Progresión de niveles
        while (currentLevel != GameLevel.Caos)
        {
            yield return new WaitForSeconds(90f);  // Espera entre niveles
            currentLevel++;
            yield return StartCoroutine(HandleLevelTransition(currentLevel));
        }
    }

    private IEnumerator HandleLevelTransition(GameLevel level)
    {
        spawningActive = false;

        string levelName = level == GameLevel.Caos ? "¡Nivel Caos!" : $" {level}";
        yield return StartCoroutine(ShowLevelCanvas(levelName, 4f));

        // Actualizar el número romano en la esquina
        if (romanLevelText != null)
        {
            romanLevelText.text = GetRomanNumeral(level);  // Mostrar solo el número romano
        }

        // Tiempo de preparación
        yield return new WaitForSeconds(10f);

        spawningActive = true;
    }

    private IEnumerator ShowLevelCanvas(string nivel, float duration)
    {
        if (nivelCanvas != null && nivelText != null)
        {
            nivelCanvas.enabled = true;
            nivelText.text = nivel;
        }

        yield return new WaitForSeconds(duration);

        // Solo se apaga el nivelCanvas cuando termina la transición de nivel
        if (nivelCanvas != null)
            nivelCanvas.enabled = false;
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (spawningActive)
            {
                Vector3 spawnPos = GetValidRandomPosition();
                GameObject enemyToSpawn = GetRandomEnemyPrefab();

                if (enemyToSpawn != null)
                {
                    GameObject enemy = Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
                    EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

                    if (enemyHealth != null)
                    {
                        if (enemyToSpawn == enemyPrefab1)
                            enemyHealth.enemyScore = 25;
                        else if (enemyToSpawn == enemyPrefab2)
                            enemyHealth.enemyScore = 75;
                        else if (enemyToSpawn == enemyPrefab3)
                            enemyHealth.enemyScore = 120;

                        enemyHealth.onDeath += OnEnemyDeath;
                    }
                }
            }

            int levelIndex = (int)currentLevel;
            float waitTime = Random.Range(minSpawnIntervals[levelIndex], maxSpawnIntervals[levelIndex]);
            yield return new WaitForSeconds(waitTime);
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float roll = Random.Range(0f, 1f);

        if (roll < 0.5f)
            return enemyPrefab1;
        else if (roll < 0.8f)
            return enemyPrefab2;
        else
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
            float distance = Vector3.Distance(enemyHealth.transform.position, Camera.main.transform.position);
            int score = enemyHealth.enemyScore;

            // Suma distancia
            score += Mathf.CeilToInt(distance);

            // Bonus en Caos
            if (currentLevel == GameLevel.Caos)
            {
                score += 100;  // Bonus por cada enemigo eliminado en Caos
            }

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

    // Función para convertir los números en números romanos
    private string GetRomanNumeral(GameLevel level)
    {
        switch (level)
        {
            case GameLevel.Nivel1: return "I";
            case GameLevel.Nivel2: return "II";
            case GameLevel.Nivel3: return "III";
            case GameLevel.Maestro: return "IV";
            case GameLevel.Leyenda: return "V";
            case GameLevel.Caos: return "VI";
            default: return "I";
        }
    }
}
