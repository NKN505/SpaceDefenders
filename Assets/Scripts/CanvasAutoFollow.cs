using UnityEngine;

public class CanvasAutoFollowVR : MonoBehaviour
{
    public Transform playerCamera;  // Arrastra aquí la Main Camera (la de VR)
    public float distanceFromCamera = 2.0f; // A qué distancia quieres el Canvas
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
            forward.y = 0; // Opcional: mantén el Canvas a nivel del suelo (sin inclinarlo hacia arriba/abajo)
            forward.Normalize();

            transform.position = playerCamera.position + forward * distanceFromCamera + canvasOffset;
            transform.LookAt(playerCamera); // Que el Canvas mire hacia la cámara
            transform.Rotate(0, 180, 0); // Porque LookAt pone el "dorso", giramos 180 grados
        }
        else
        {
            Debug.LogWarning("CanvasAutoFollowVR: No se encontró cámara principal.");
        }
    }
}
