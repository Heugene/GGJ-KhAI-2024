using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // Швидкість польоту у стрибку
    [SerializeField]
    private float maxJumpDistance = 5f; // Максимальна дальність стрибку
    private float chargedJumpDistance = 0; // Поточний заряд стрибку (У еквіваленті відстані)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // Приріст заряду
    [SerializeField]
    private float DashSpeed = 12;
    private float DashSpeedTemp = 0;
    [SerializeField]
    private float DashCooldown = 1;
    private float DashCooldownTemp = 0;
    [SerializeField]
    private float DashSpeedReducer = 0.05f;
    [SerializeField]
    private ItemType item = ItemType.None;

    private Vector2 mousePosition; // Координати миші
    private Vector2 LastMousePosition; // Останні координати миші
    private bool IsMoving = false; // Прапорець стану руху
    private bool IsButtonJumpPressed = false; // Прапорець утримання кнопки руху
    private bool isPlayerHitEnemy = false; // змінна для визначення чи гравець зіткнувся з ворогом
    [SerializeField]
    private bool isCanDash = false; // может ли игрок сделать деш
    [SerializeField]
    private bool isDashing = false; // делает ли игрок деш


    private void Start()
    {
        DashSpeedTemp = DashSpeed;
    }

    // логика
    void FixedUpdate()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        if (item == ItemType.Ketchup)
        {
            if(isCanDash)
                MakeDash();

            if (isDashing)
            {
                CalculateDash();
                CalculateDashReload();
            }
        }

        CalculateJumpDistance();

        if (IsMoving && !isPlayerHitEnemy && isCanDash == false)
            MoveToTarget(LastMousePosition);// Перемещаем персонаж}
    }

    // input процесы
    void Update()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        // Додали підтримку ЛКМ
        // Якщо натискаємо кнопку пересування
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonJumpPressed = true;
        }

        // Якщо відпустили кнопку пересування
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && isDashing == false)
        {
            isCanDash = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;
            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1) && item == ItemType.Ketchup && !isDashing)
        {
            isCanDash = true;
            isDashing = true;
        }
    }

    // зарядка прыжка
    void CalculateJumpDistance()
    {
        if (!IsMoving && IsButtonJumpPressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }
    }

    // уменьшение скорости деша на заданое значение DashSpeedReducer
    void CalculateDash()
    {
        if (isCanDash && isDashing)
        {
            DashSpeed -= DashSpeedReducer;
            if(DashSpeed <= 0)
            {
                isCanDash = false;
                DashSpeed = DashSpeedTemp;
            }
        }
    }

    // перезарядка деша в секундах
    void CalculateDashReload()
    {
        if (!isCanDash)
        {
            DashCooldownTemp += Time.fixedDeltaTime;
            if (DashCooldownTemp >= DashCooldown)
            {
                DashCooldownTemp = 0;
                isDashing = false;
            }
        }
    }

    // Метод пересування до заданої точки
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.Lerp(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // Создание деша
    void MakeDash()
    {
        // Вычисляем вектор направления от текущей позиции до позиции мыши
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Если длина вектора меньше 5, то не изменяем его, иначе ограничиваем до 5
        if (direction.magnitude > 5f)
            direction = direction.normalized * 5f;

        // Применяем MoveTowards для движения к курсору
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction, DashSpeed * Time.deltaTime);

        // Обновляем LastMousePosition
        LastMousePosition = transform.position;
    }

    // Функція, що повертає світові координати миші
    Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // остановка игрока когда он сталкиваеться с противником
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
}