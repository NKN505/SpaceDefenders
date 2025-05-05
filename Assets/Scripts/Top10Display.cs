using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Top10Display : MonoBehaviour
{
    public Transform scoreContainer;
    public GameObject scoreEntryPrefab;

    void Start()
    {
        var scores = ScoreManager.Instance.topScores;

        for (int i = 0; i < scores.Count && i < 10; i++)
        {
            var entry = scores[i];
            GameObject go = Instantiate(scoreEntryPrefab, scoreContainer);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.alignment = TextAlignmentOptions.Center;
                text.text = $"{i + 1}. {entry.name} - {entry.score}";

                // Escala el tamaño del texto según la posición
                if (i == 0) text.fontSize = 46;  // Top 1
                else if (i == 1) text.fontSize = 42;  // Top 2
                else if (i == 2) text.fontSize = 40;  // Top 3
                else text.fontSize = 36;  // Resto
            }

            // Asegura que el objeto se adapte al layout
            LayoutElement layout = go.GetComponent<LayoutElement>();
            if (layout == null)
                layout = go.AddComponent<LayoutElement>();

            layout.preferredHeight = 80;
        }
    }
}