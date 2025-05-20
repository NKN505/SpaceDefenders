using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class Top10Display : MonoBehaviour
{
    public Transform scoreContainer;
    public GameObject scoreEntryPrefab;

    /// <summary>
    /// En Start() hacemos dos cosas:
    ///  1) Descargamos el Top 10 desde Talo (si existe conexión).
    ///  2) Sobreescribimos ScoreManager.Instance.topScores con lo obtenido online.
    ///  3) Mostramos la lista resultante (online si hubo éxito, local si falla).
    /// </summary>
    async void Start()
    {
        // 1) Intentar recuperar Top 10 del servidor
        List<ScoreEntry> onlineTop = null;
        try
        {
            onlineTop = await OnlineScoreManager.GetTopScoresAsync();
        }
        catch
        {
            // Si hay algún fallo (sin internet, sin identificación, CORS, etc.), onlineTop quedará null o vacío
            onlineTop = new List<ScoreEntry>();
        }

        // 2) Si obtuvimos puntuaciones online, actualizamos la lista local
        if (onlineTop != null && onlineTop.Count > 0)
        {
            ScoreManager.Instance.topScores = onlineTop;
            // Opcional: guardar este Top 10 como local para la siguiente vez
            // Para ello podrías exponer un método público en ScoreManager:
            // ScoreManager.Instance.SaveScoresLocal();
        }

        // 3) Mostrar el listado (online si hubo datos; en caso contrario, el local que ya tenía ScoreManager)
        List<ScoreEntry> scoresParaMostrar = ScoreManager.Instance.topScores;

        // Limpiar cualquier hijo previo que haya en el contenedor
        foreach (Transform hijo in scoreContainer)
        {
            Destroy(hijo.gameObject);
        }

        // Instanciar hasta 10 elementos
        for (int i = 0; i < scoresParaMostrar.Count && i < 10; i++)
        {
            var entry = scoresParaMostrar[i];
            GameObject go = Instantiate(scoreEntryPrefab, scoreContainer);
            TMP_Text text = go.GetComponentInChildren<TMP_Text>();

            if (text != null)
            {
                text.alignment = TextAlignmentOptions.Center;
                text.text = $"{i + 1}. {entry.name} - {entry.score}";

                if (i == 0)        text.fontSize = 46;
                else if (i == 1)   text.fontSize = 42;
                else if (i == 2)   text.fontSize = 40;
                else               text.fontSize = 36;
            }

            LayoutElement layout = go.GetComponent<LayoutElement>();
            if (layout == null)
                layout = go.AddComponent<LayoutElement>();

            layout.preferredHeight = 80;
        }
    }
}
