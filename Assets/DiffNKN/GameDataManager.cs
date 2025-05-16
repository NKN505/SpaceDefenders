using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager Instance;

    [HideInInspector] public string playerName = "Jugador";
    [HideInInspector] public int playerScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Métodos para modificar datos si quieres, por ejemplo:
    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetPlayerScore(int score)
    {
        playerScore = score;
    }
}
