using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
