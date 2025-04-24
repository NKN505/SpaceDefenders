using UnityEngine;
using TMPro;

public class ScoreCount : MonoBehaviour
{
    public int totalScore = 0;
    public TextMeshProUGUI scoreText;

    public void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = totalScore.ToString();
        }
    }
}
