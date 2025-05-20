using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using TaloGameServices;

public static class OnlineScoreManager
{
    private const string leaderboardName = "Score";
    private const int MaxHistorico = 50;
    private const int TopVisibles = 10;

    /// <summary>
    /// Guarda una puntuación online en Talo (con la propiedad "playerName").
    /// </summary>
    public static async Task SaveScoreOnline(string name, int score)
    {
        try
        {
            // Asegurarnos de que el jugador está identificado en Talo
            Talo.IdentityCheck();

            // Añadir la entrada al leaderboard enviando la propiedad "playerName"
            await Talo.Leaderboards.AddEntry(
                leaderboardName,
                score,
                ("playerName", name)
            );
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error al guardar la puntuación online: " + e.Message);
        }
    }

    /// <summary>
    /// Obtiene las 10 mejores puntuaciones del leaderboard "global_leaderboard".
    /// </summary>
    public static async Task<List<ScoreEntry>> GetTopScoresAsync()
    {
        var listado = new List<ScoreEntry>();

        try
        {
            // Recuperar la primera página (hasta 50 entradas) desde Talo
            LeaderboardEntriesResponse response =
                await Talo.Leaderboards.GetEntries(leaderboardName, new GetEntriesOptions { page = 0 });

            if (response.entries == null)
                return listado;

            // De todas las entradas, mapear a ScoreEntry (usando la clase definida en ScoreManager.cs)
            listado = response.entries
                .Select(e => new ScoreEntry
                {
                    name = e.GetProp("playerName", "SinNombre"),
                    score = (int)e.score
                })
                // Ordenar descendente por puntuación
                .OrderByDescending(se => se.score)
                // Tomar sólo las 10 mejores
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
