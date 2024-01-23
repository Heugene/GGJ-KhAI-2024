using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // Швидкість польоту у стрибку
    [SerializeField]
    private float dashSpeed = 7f; // Швидкість польоту у стрибку
    private float dashSpeedTemp = 0;
    [SerializeField]
    private float dashReloadTime = 1f;
    [SerializeField]
    private float maxDashTime = 5f; 
    [SerializeField]
    private float maxJumpDistance = 5f; // Максимальна дальність стрибку
    [SerializeField]
    private float chargedJumpDistance = 0; // Поточний заряд стрибку (У еквіваленті відстані)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // Приріст заряду
    [SerializeField]
    private ItemType itemEquipped;

    private float dashTickTimeToReload = 0;
    private float forDashReaload = 0;

    private Vector2 mousePosition; // Координати миші
    private Vector2 LastMousePosition; // Останні координати миші
    private bool isMoving = false; // Прапорець стану руху
    private bool IsButtonJumpPressed = false; // Прапорець утримання кнопки руху
    private bool isPlayerHitEnemy = false; // змінна для визначення чи гравець зіткнувся з ворогом
    private bool isCanDashing = false;
    private bool isRKMPressed = false;

    private void Start()
    {
        dashSpeedTemp = dashSpeed;
    }

    void FixedUpdate()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        if(itemEquipped == ItemType.Ketchup)
        {
            calculateTimeOfDash();
            ReloadDash();

            if (isCanDashing)
            {
                // дешим персонаж
                makeDash();
            }
        }

        calculateJumpDistanceCharge();

        if (isMoving && !isPlayerHitEnemy && isCanDashing == false)
        {
            // Перемещаем персонаж
            MoveToTarget(LastMousePosition);
        }
    }

    void Update()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        // Додали підтримку ЛКМ
        // Якщо натискаємо кнопку пересування
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            isMoving = false;
            IsButtonJumpPressed = true;
        }

        // Якщо відпустили кнопку пересування
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            isMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;
            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && itemEquipped == ItemType.Ketchup)
        {
            isCanDashing = true;
            isRKMPressed = true;
        }
    }




    void calculateTimeOfDash()
    {
        if (isCanDashing == true && isRKMPressed)
        {
            dashTickTimeToReload += Time.fixedDeltaTime;
            dashSpeed -= 0.05f;
            if (dashTickTimeToReload >= maxDashTime || dashSpeed <= 0)
            {
                dashTickTimeToReload = 0f;
                isCanDashing = false;
                isRKMPressed = false;
                LastMousePosition = transform.position;
                dashSpeed = dashSpeedTemp;
            }
        }
    }

    void ReloadDash()
    {
        if (isCanDashing == false && isRKMPressed)
        {
            forDashReaload += Time.fixedDeltaTime;
            if (forDashReaload >= dashReloadTime)
            {
                forDashReaload = 0f;
                isCanDashing = true;
            }
        }
    }

    void calculateJumpDistanceCharge()
    {
        if (!isMoving && IsButtonJumpPressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }
    }

    /// Метод пересування до заданої точки
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.Lerp(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    /// Функція, що повертає світові координати миші
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    /// остановка игрока когда он сталкиваеться с противником
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
        }
    }

    private void makeDash()
    {
        // Применяем перемещение в сторону курсора с постоянной скоростью
        transform.position = Vector2.MoveTowards(transform.position, mousePosition, Time.deltaTime * dashSpeed);
    }
}
