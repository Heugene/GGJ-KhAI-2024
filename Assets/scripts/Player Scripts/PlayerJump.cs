using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    PlayerDash playerDash;

    [SerializeField] private float moveSpeed = 7f; // Швидкість польоту у стрибку
    [SerializeField] private float maxJumpDistance = 5f; // Максимальна дальність стрибку
    [SerializeField] private float maxJumpReload = 0.5f;
    [SerializeField] private float chargedJumpDistanceMultiplier = 0.4f; // Приріст заряду

    private float currentJumpReloadValue = 0f;
    private float chargedJumpDistance = 0; // Поточний заряд стрибку (У еквіваленті відстані)

    private Vector2 mousePosition; // Координати миші
    private Vector2 LastMousePosition; // Останні координати миші

    public bool isCanMove = true;
    public bool IsMoving = false; // Прапорець стану руху
    public bool IsButtonJumpPressed = false; // Прапорець утримання кнопки руху
    public bool isPlayerHitEnemy = false; // змінна для визначення чи гравець зіткнувся з ворогом


    private void Start()
    {
        playerDash = GetComponent<PlayerDash>();
    }

    private void FixedUpdate()
    {
        CalculateJumpDistance();
        CalculateJumpReload();

        if (!isCanMove && IsMoving && !isPlayerHitEnemy && playerDash.isCanDash == false)
            MoveToTarget(LastMousePosition);// Перемещаем персонажа
    }

    private void Update()
    {
        mousePosition = GetMouseWorldPosition();
        MouseChargingInput();
        JumpWhenMouseUP();
        StopMoveWhenCloseToPosition();
    }

    // зарядка прыжка при зажатом пробеле или LKM
    void MouseChargingInput()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0) && IsMoving == false && isCanMove == true)
        {
            IsButtonJumpPressed = true;
        }
    }

    // остановить деш когда близко к последней позиции курсора
    void StopMoveWhenCloseToPosition()
    {
        var Direction = LastMousePosition - (Vector2)transform.position;

        if (Direction.magnitude < 0.1)
        { 
            IsMoving = false;
        }
    }

    // Если отпустить LKM то начинаем прыжок
    void JumpWhenMouseUP()
    {
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && playerDash.isDashing == false && IsMoving == false && isCanMove == true)
        {
            playerDash.isCanDash = false;
            isCanMove = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;

            // Вычисляем вектор направления от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Ограничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
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

    // перезарядка прыжка
    void CalculateJumpReload()
    {
        if (!IsMoving)
        {
            currentJumpReloadValue += Time.fixedDeltaTime;
            if (currentJumpReloadValue >= maxJumpReload)
            {
                currentJumpReloadValue = 0;
                isCanMove = true;
            }
        }
    }

    // Метод пересування до заданої точки
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Применяем перемещение вдоль вектора направления с постоянной скоростью
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // Функція, що повертає світові координати миші
    public Vector2 GetMouseWorldPosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    // остановка игрока когда он сталкиваеться с противником
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            isPlayerHitEnemy = true;
            IsMoving = false;
        }

        if (collision.transform.tag == "barrel")
            IsMoving = false;
    }

}
