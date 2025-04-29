using UnityEngine;

public class CanvasAutoFollowVR : MonoBehaviour
{
    public Transform playerCamera;  // Arrastra aqu� la Main Camera (la de VR)
    public float distanceFromCamera = 2.0f; // A qu� distancia quieres el Canvas
    public Vector3 canvasOffset = Vector3.zero; // Por si quieres ajustar altura o desplazamiento lateral

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main.transform;
        }

        if (playerCamera != null)
        {
            Vector3 forward = playerCamera.forward;
            forward.y = 0; // Opcional: mant�n el Canvas a nivel del suelo (sin inclinarlo hacia arriba/abajo)
            forward.Normalize();

            transform.position = playerCamera.position + forward * distanceFromCamera + canvasOffset;
            transform.LookAt(playerCamera); // Que el Canvas mire hacia la c�mara
            transform.Rotate(0, 180, 0); // Porque LookAt pone el "dorso", giramos 180 grados
        }
        else
        {
            Debug.LogWarning("CanvasAutoFollowVR: No se encontr� c�mara principal.");
        }
    }
}
