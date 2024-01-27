using System;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] GameObject settingsObj = null;


    public void ChangeVisibility()
    {
        if (settingsObj != null)
        {
            settingsObj.SetActive(!settingsObj.activeSelf);
        }
        else
        {
             throw new Exception("ќб'Їект Settings не знайдений.");
        }
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
