using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGroupLogic : MonoBehaviour
{
    internal event Action LevelCompleted; // подія проходження баттон групи

    // Загальновидима властивість, яка дозволяє визначити, чи пройдена дана зона з кнопками (потрібна для одноразового проходження)
    internal bool Passed { get; private set; }

    // Властивість, яка повертає True, тільки якщо всі кнопки цієї групи в момент часу натиснуті
    private bool Activated 
    {
        get => Array.TrueForAll(GetComponentsInChildren<ButtonPressLogic>(), x => x.isPressed.Equals(true));
    }

    private void FixedUpdate()
    {
        // Якщо в момент часу всі кнопки затиснуті одночасно, і рівень ще не проходився, помічаємо його як пройдений.
        if (Activated && Passed == false) 
        {
            Passed = true;
            LevelCompleted?.Invoke();
        }
    }
}
