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

        if (IsMoving)
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
            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }
    }

    // Метод пересування до заданої точки
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // Функція, що повертає світові координати миші
    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
