using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthController : MonoBehaviour
{
    public int health { get; private set; } = 5; // Поточне ХП
    public bool isImmuneToDamage = false;
    private int numOfParts; // Кількість частинок сосиски, яка репрезентує кількість ХП;
    private Image[] parts; // Массив частинок сосиски
    private Animator anim; // Аніматор плеєра

    [SerializeField] private Sprite fullBodyPart; // Спрайт заповненої основної частинки
    [SerializeField] private Sprite emptyBodyPart; // Спрайт порожньої основної частинки
    [SerializeField] private Sprite fullTailPart; // Спрайт заповненої хвостової частинки
    [SerializeField] private Sprite emptyTailPart; // Спрайт порожньої хвостової частинки
    [SerializeField] private Sprite fullHeadPart; // Спрайт заповненої головної частинки
    [SerializeField] private Sprite emptyHeadPart; // Спрайт порожньої головної частинки


    // Метод отримання дамагу
    internal void TakeDamage(int damage)
    {
        if (isImmuneToDamage) return;

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
        // Виконуємо анімацію смерті
        anim.SetBool("isDead", true);

        // Зупиняємо логіку гравця
        GetComponent<PlayerAnimationController>().Freeze(true);

       StartCoroutine( BackToMenu() );
    }

    // Перехід в меню з затримкою
    private IEnumerator BackToMenu()
    {
        // Дістаємо затримку анімації
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        
        // Відтворюємо перехід в меню
        SceneManager.LoadScene(0);
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        numOfParts = health;
        parts = healthBar.GetComponentsInChildren<Image>();
        anim = GetComponent<Animator>();
    }
}
