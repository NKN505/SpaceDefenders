using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance;

    public string playerName;
    public int score;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persiste entre escenas
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }
    }
}
