using System;
using UnityEngine;
using TaloGameServices;

public class AuthManager : MonoBehaviour
{
    async void Start()
    {
        // Generar o recuperar un ID único para este dispositivo/jugador
        string jugadorId = PlayerPrefs.GetString("talo-id", Guid.NewGuid().ToString());

        try
        {
            // Identificar al jugador usando el servicio "custom"
            await Talo.Players.Identify("custom", jugadorId);

            // Guardar el ID para la próxima vez
            PlayerPrefs.SetString("talo-id", jugadorId);
            PlayerPrefs.Save();

            Debug.Log($"Jugador identificado en Talo con ID = {jugadorId}");
        }
        catch (Exception e)
        {
            Debug.LogError("Error al identificar al jugador en Talo: " + e.Message);
        }
    }
}
