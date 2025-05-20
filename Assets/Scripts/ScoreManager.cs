using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    private const int MaxTopScores = 10;
    private const string SaveKey = "TopScores";

    public List<ScoreEntry> topScores = new List<ScoreEntry>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeScores()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Sin conexión. Usando ranking local.");
            LoadScores();
        }
        else
        {
            LoadOnlineTopScores();
        }
    }

    /// <summary>
    /// Guarda una puntuación (local + online).
    /// </summary>
    public void SaveScore(string name, int score)
    {
        // 1) Guardar localmente
        topScores.Add(new ScoreEntry { name = name, score = score });
        topScores = topScores
            .OrderByDescending(s => s.score)
            .Take(MaxTopScores)
            .ToList();
        SaveScoresToPrefs();

        // 2) Guardar online (no await para no bloquear la UI)
        _ = OnlineScoreManager.SaveScoreOnline(name, score);
    }

    /// <summary>
    /// Carga las 10 mejores puntuaciones desde Talo y reemplaza la lista local.
    /// </summary>
    public async void LoadOnlineTopScores()
    {
        var onlineTop = await OnlineScoreManager.GetTopScoresAsync();
        topScores = onlineTop;

        Debug.Log("Top Scores Online:");
        foreach (var entry in topScores)
        {
            Debug.Log($"{entry.name}: {entry.score}");
        }
    }

    void SaveScoresToPrefs()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { scores = topScores });
        PlayerPrefs.SetString(SaveKey, json);
        PlayerPrefs.Save();
    }

    void LoadScores()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (!string.IsNullOrEmpty(json))
        {
            var wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
            if (wrapper != null && wrapper.scores != null)
            {
                topScores = wrapper.scores;
                Debug.Log("Top Scores cargados localmente:");
                foreach (var s in topScores)
                {
                    Debug.Log($"{s.name} - {s.score}");
                }
            }
            else
            {
                Debug.LogWarning("No se pudo deserializar el wrapper o lista vacía");
            }
        }
        else
        {
            Debug.Log("No hay datos previos guardados en PlayerPrefs.");
        }
    }

    [System.Serializable]
    class ScoreListWrapper
    {
        public List<ScoreEntry> scores;
    }
}
