using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public TMP_InputField nameInput; // Asume que usas TextMeshPro
    public void OnStartGame()
    {
        GameManager.Instance.playerName = nameInput.text;
        SceneManager.LoadScene("GameScene");
    }
}
