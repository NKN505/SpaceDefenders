using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void OnStartGame()
    {
        if (GameDataManager.Instance != null && nameInput != null)
        {
            string nombre = nameInput.text.Trim();
            GameDataManager.Instance.SetPlayerName(nombre);
        }

        SceneManager.LoadScene(2);
    }
}
