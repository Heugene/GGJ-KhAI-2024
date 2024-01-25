using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject settingsObj = null;
    
    public void ChangeVisibility(bool visible)
    {
        if (settingsObj != null)
        {
            settingsObj.SetActive(visible);
        }
        else
        {
             throw new Exception("Объект Settings не найден.");
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
