using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressLogic : MonoBehaviour
{
    private bool isPressed = false; // Чи натиснута кнопка
    private Animator anim;

    private void Start()
    {
        // Дістаємо аніматор з кнопки, на яку повісили скрипт
        anim = GetComponent<Animator>();
    }

    // Якщо тригериться колайдер кнопки
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Змінюємо відповідний прапорець на true 
        isPressed = true;
        Debug.Log("Button state = " + isPressed);

        // Підключаємо тут анімацію
        anim.SetBool("Pressed", isPressed);
    }

    // Якщо колайдер кнопки перестає тригеритися
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Змінюємо відповідний прапорець на false
        isPressed = false;
        Debug.Log("Button state = " + isPressed);

        // Підключаємо тут анімацію
        anim.SetBool("Pressed", isPressed);
    }
}
