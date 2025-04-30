using TMPro;
using UnityEngine;

public class Top10Display : MonoBehaviour
{
    public Transform scoreContainer;
    public GameObject scoreEntryPrefab;

    void Start()
    {
        var scores = ScoreManager.Instance.topScores;
        foreach (var entry in scores)
        {
            GameObject go = Instantiate(scoreEntryPrefab, scoreContainer);
            go.GetComponentInChildren<TMP_Text>().text = $"{entry.name} - {entry.score}";
        }
    }
}
