using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class TypewriterEffect : MonoBehaviour
{
    [SerializeField] private float typingSpeed = 1f;
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private string fullText = "Estamos en el año 2525..."; // Tu texto aquí

    private void Start()
    {
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        textComponent.text = ""; // Limpia el texto inicial
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Cuando termina el texto, carga la escena
        LoadGameScene();
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene(3); // Carga la escena con índice 3
    }
}