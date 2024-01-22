using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // Швидкість польоту у стрибку
    [SerializeField]
    private float maxJumpDistance = 5f; // Максимальна дальність стрибку
    [SerializeField]
    private float chargedJumpDistance = 0; // Поточний заряд стрибку (У еквіваленті відстані)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // Приріст заряду

    private Vector2 mousePosition; // Координати миші
    private Vector2 LastMousePosition; // Останні координати миші
    private bool IsMoving = false; // Прапорець стану руху
    private bool IsButtonMovePressed = false; // Прапорець утримання кнопки руху
    private bool isPlayerHitEnemy = false; // змінна для визначення чи гравець зіткнувся з ворогом

    void FixedUpdate()
    {
        // Получаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        if (!IsMoving && IsButtonMovePressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }

        if (IsMoving && !isPlayerHitEnemy)
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
            IsMoving = false;
            IsButtonMovePressed = true;
        }

        // Якщо відпустили кнопку пересування
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            IsMoving = true;
            IsButtonMovePressed = false;
            isPlayerHitEnemy = false;
            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }
    }

    /// <summary>
    /// Метод пересування до заданої точки
    /// </summary>
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.Lerp(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Функція, що повертає світові координати миші
    /// </summary>
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }

    /// <summary>
    /// остановка игрока когда он сталкиваеться с противником
    /// </summary>
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
