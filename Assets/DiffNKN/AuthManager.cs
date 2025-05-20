using System;
using UnityEngine;
using TaloGameServices;

public class AuthManager : MonoBehaviour
{
    async void Start()
    {
        // Genera o recupera un ID único para este dispositivo/jugador
        string jugadorId = PlayerPrefs.GetString("talo-id", Guid.NewGuid().ToString());

        try
        {
            // Identifica al jugador en Talo usando el servicio "custom"
            await Talo.Players.Identify("custom", jugadorId);

            // Guarda el ID para la próxima vez
            PlayerPrefs.SetString("talo-id", jugadorId);
            Debug.Log($"Jugador identificado en Talo con ID = {jugadorId}");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al identificar al jugador en Talo: " + e.Message);
        }
    }
}
