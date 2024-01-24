using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public int health = 5; // Поточне ХП
    private int numOfParts; // Кількість частинок сосиски, яка репрезентує кількість ХП;

    public Image[] parts; // Массив частинок сосиски

    public Sprite fullBodyPart; // Спрайт заповненої основної частинки
    public Sprite emptyBodyPart; // Спрайт порожньої основної частинки
    public Sprite fullTailPart; // Спрайт заповненої хвостової частинки
    public Sprite emptyTailPart; // Спрайт порожньої хвостової частинки
    public Sprite fullHeadPart; // Спрайт заповненої головної частинки
    public Sprite emptyHeadPart; // Спрайт порожньої головної частинки


    // Метод отримання дамагу
    internal void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage; // Віднімаємо дамаг від ХП

            if (health <= 0) // Якщо 0 чи менше ХП, помираємо і виходимо з методу
            {
                health = 0;
                RefreshHealthDisplay();
                Die();
                return;
            }
            RefreshHealthDisplay();

        }
    }

    // Метод відновлення ХП
    internal void Heal(int healAmount)
    {
        if (health > 0)
        {
            health += healAmount; // Відновлюємо ХП на задане число

            // перевірка, щоб не можна було відновити більше максимуму ХП.
            if (health > numOfParts)
            {
                health = numOfParts;
            }

            RefreshHealthDisplay();
        }
    }
    
    // Метод оновлення відображення ХП
    internal void RefreshHealthDisplay()
    {
        for (int i = 0; i < health; i++)
        {
            if (i == 0)
            {
                parts[i].sprite = fullTailPart;
            }
            else if (i == numOfParts - 1)
            {
                parts[i].sprite = fullHeadPart;
            }
            else 
            {
                parts[i].sprite = fullBodyPart;
            }
        }
        for (int i = health; i < numOfParts; i++)
        {
            if (i == 0)
            {
                parts[i].sprite = emptyTailPart;
            }
            else if (i == numOfParts - 1)
            {
                parts[i].sprite = emptyHeadPart;
            }
            else
            {
                parts[i].sprite = emptyBodyPart;
            }
        }
    }

    // Помираєм
    internal void Die()
    {
        // Виконуємо якісь дії після смерті
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        numOfParts = health;
        parts = healthBar.GetComponentsInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
