using UnityEngine;
using System.Threading.Tasks;
using Supabase;
using Supabase.Gotrue;

public class ScoreUploader : MonoBehaviour
{
    public static ScoreUploader Instance;

    private Supabase.Client client;

    // Configura aquí tu URL y API KEY de Supabase
    [Header("Supabase Config")]
    public string supabaseUrl = "https://xvilhphextfpkcsnmizq.supabase.co";
    public string supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Inh2aWxocGhleHRmcGtjc25taXpxIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NDczMTkwMzgsImV4cCI6MjA2Mjg5NTAzOH0.ASQK3YWS_xv4Xr-AMsgPI-sj3c0-Nu-ORc-5J2AnjKE";

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            await InitializeSupabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private async Task InitializeSupabase()
    {
        client = new Supabase.Client(supabaseUrl, supabaseKey);
        await client.InitializeAsync();
        Debug.Log("Supabase inicializado correctamente.");
    }

    public async void EnviarPuntuacion(string nombre, int puntuacion)
    {
        if (client == null)
        {
            Debug.LogWarning("Cliente Supabase no inicializado todavía.");
            return;
        }

        var tablaPuntuaciones = client.From<Puntuacion>();

        var nuevaPuntuacion = new Puntuacion
        {
            nombre = nombre,
            puntuacion = puntuacion,
            fecha = System.DateTime.UtcNow
        };

        var response = await tablaPuntuaciones.Insert(nuevaPuntuacion);

        if (response != null)
        {
            Debug.Log($"Puntuación enviada: {nombre} - {puntuacion}");
        }
        else
        {
            Debug.LogWarning("Error al enviar puntuación.");
        }
    }
}

// Clase para mapear la tabla de Supabase
public class Puntuacion
{
    public string id { get; set; }  // Supabase genera el UUID automáticamente
    public string nombre { get; set; }
    public int puntuacion { get; set; }
    public System.DateTime fecha { get; set; }
}
