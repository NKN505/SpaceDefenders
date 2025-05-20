using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TaloGameServices;

public static class OnlineScoreManager
{
    private const string leaderboardName = "Score";    // Debe coincidir EXACTAMENTE con el Internal Name del leaderboard en Talo
    private const int TopVisibles = 10;

    /// <summary>
    /// Envía la puntuación al leaderboard “Score” de Talo. 
    /// Lanza excepción si no hay sesión identificada.
    /// </summary>
    public static async Task SaveScoreOnline(string name, int score)
    {
        try
        {
            // Verificar que Player ha sido identificado (lanzará excepción si no)
            Talo.IdentityCheck();

            // Llamada asíncrona para añadir la entrada
            await Talo.Leaderboards.AddEntry(
                leaderboardName,
                score,
                ("playerName", name)
            );

            Debug.Log($"Puntuación online guardada: {name} - {score}");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar la puntuación online: " + e.Message);
        }
    }

    /// <summary>
    /// Recupera el Top 10 del leaderboard “Score” desde Talo.
    /// </summary>
    public static async Task<List<ScoreEntry>> GetTopScoresAsync()
    {
        var listado = new List<ScoreEntry>();

        try
        {
            // Obtener la primera página (hasta 50 entradas) desde Talo
            var response = await Talo.Leaderboards.GetEntries(
                leaderboardName,
                new GetEntriesOptions { page = 0 }
            );

            if (response.entries == null)
                return listado;

            listado = response.entries
                .Select(e => new ScoreEntry
                {
                    name  = e.GetProp("playerName", "SinNombre"),
                    score = (int)e.score
                })
                .OrderByDescending(se => se.score)
                .Take(TopVisibles)
                .ToList();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al obtener las puntuaciones online: " + e.Message);
        }

        return listado;
    }
}
