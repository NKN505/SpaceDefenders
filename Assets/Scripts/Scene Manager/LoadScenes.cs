using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    public void ElegirEscena(int scene)

    {
        SceneManager.LoadScene(scene);

    }

    public void MenuInicial()
    {
        SceneManager.LoadScene("Scenes/Level");
    }
    public void Configuration()
    {
        SceneManager.LoadScene("Scenes");
    }

    public void salir()
    {
        Application.Quit();
    }
}