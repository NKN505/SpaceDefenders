using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public List<ScoreEntry> topScores = new List<ScoreEntry>();

    private const string PlayerPrefsKey = "TopScores";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadScores();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Añade la puntuación a la lista local (ordenada) y la persiste en PlayerPrefs.
    /// Luego envía inmediatamente esa misma puntuación al servidor (Talo).
    /// </summary>
    public async void SaveScore(string playerName, int score)
    {
        // 1) Guardar local: añadir, ordenar, recortar a 10
        AddScoreLocal(playerName, score);
        SaveScoresLocal();

        // 2) Enviar al servidor Talo
        await OnlineScoreManager.SaveScoreOnline(playerName, score);
    }

    /// <summary>
    /// Solo añade la entrada a la lista local y la ordena,
    /// sin persistir ni enviar al servidor.
    /// </summary>
    private void AddScoreLocal(string playerName, int score)
    {
        topScores.Add(new ScoreEntry { name = playerName, score = score });
        topScores.Sort((a, b) => b.score.CompareTo(a.score));
        if (topScores.Count > 10)
        {
            topScores.RemoveAt(topScores.Count - 1);
        }
    }

    /// <summary>
    /// Escribe la lista topScores en PlayerPrefs (JSON).
    /// </summary>
    private void SaveScoresLocal()
    {
        string json = JsonUtility.ToJson(new ScoreListWrapper { scores = topScores });
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Carga desde PlayerPrefs la lista guardada previamente.
    /// </summary>
    private void LoadScores()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsKey))
        {
            string json = PlayerPrefs.GetString(PlayerPrefsKey);
            ScoreListWrapper wrapper = JsonUtility.FromJson<ScoreListWrapper>(json);
            if (wrapper != null && wrapper.scores != null)
            {
                topScores = wrapper.scores;
            }
        }
    }

    [System.Serializable]
    private class ScoreListWrapper
    {
        public List<ScoreEntry> scores;
    }
}

[System.Serializable]
public class ScoreEntry
{
    public string name;
    public int score;
}
