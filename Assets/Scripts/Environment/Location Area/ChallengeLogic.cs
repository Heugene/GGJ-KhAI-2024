using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeLogic : MonoBehaviour
{
    internal bool Solved { get; private set; }
    
    // Об'єкти скриптів для зон з кнопками, щоб можна було з них витягувати стан проходження зони
    [SerializeField] private ButtonGroupLogic Area1; 
    [SerializeField] private ButtonGroupLogic Area2;
    [SerializeField] private ButtonGroupLogic Area3;
    [SerializeField] private ButtonGroupLogic Area4;

    // Start is called before the first frame update
    void Start()
    {
        Area1.LevelCompleted += Area1_CompleteActions;
        Area2.LevelCompleted += Area2_CompleteActions;
        Area3.LevelCompleted += Area3_CompleteActions;
        Area4.LevelCompleted += Area4_CompleteActions;

        Area2.gameObject.SetActive(false);
        Area3.gameObject.SetActive(false);
        Area4.gameObject.SetActive(false);
    }


    // Додати перемикання камери, анімашки, розблокування соусів, видати соуси гравцю


    // Дії, коли пройшли першу зону з кнопками
    private void Area1_CompleteActions()
    {
        Debug.Log("Area1 COMPLETED");

        // Розблокуємо зону 2
        Area2.gameObject.SetActive(true);

        //Тест
        //GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();

    }

    // Дії, коли пройшли другу зону з кнопками
    private void Area2_CompleteActions()
    {
        Debug.Log("Area2 COMPLETED");

        // Розблокуємо зону 3
        Area3.gameObject.SetActive(true);
    }

    // Дії, коли пройшли третю зону з кнопками
    private void Area3_CompleteActions()
    {
        Debug.Log("Area3 COMPLETED");

        // Розблокуємо зону 4
        Area4.gameObject.SetActive(true);
    }

    // Дії, коли пройшли четверту зону з кнопками
    private void Area4_CompleteActions()
    {
        Debug.Log("Area4 COMPLETED");
        // ЯК подолати клоуна.пнг Джоджореференс.джипег, стартуємо малювання пентаграми ЛЕТСФАКІНГОООООООО
        GameObject.FindGameObjectWithTag("Pentagram").GetComponent<PentagramLogic>().Activation();
    }
}
