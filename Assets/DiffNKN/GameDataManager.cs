using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance { get; private set; }

    [HideInInspector] public string playerName = "Jugador";
    [HideInInspector] public int playerScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("GameDataManager duplicado detectado y destruido.");
            Destroy(gameObject);
        }
    }

    public void SetPlayerName(string name)
    {
        if (!string.IsNullOrWhiteSpace(name))
            playerName = name;
        else
            Debug.LogWarning("Nombre de jugador no válido asignado.");
    }

    public void SetPlayerScore(int score)
    {
        if (score >= 0)
            playerScore = score;
        else
            Debug.LogWarning("Puntuación no válida asignada (debe ser >= 0).");
    }
}
