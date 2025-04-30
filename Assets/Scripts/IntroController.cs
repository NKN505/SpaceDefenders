using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public TMP_InputField nameInput;
    public void OnStartGame()
    {
        GameManager.Instance.playerName = nameInput.text;
        SceneManager.LoadScene("Level");
    }
}
