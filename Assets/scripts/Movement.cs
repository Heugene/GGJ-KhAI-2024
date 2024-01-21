using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    // сила прыжка
    public float jumpForceMultiplier = 5f;
    // максимальная сила прыжка
    public float maxJumpForce = 20f;
    // максимальная дистанция на которую может прыгнуть герой
    public float stoppingDistance = 2f;

    // Заряжен ли на максимум прыжок
    private bool isChargingJump = false;
    private float jumpForce;

    void Update()
    {
        // Запускаем зарядку прыжка при удерживании клавиши Пробел
        if (Input.GetKey(KeyCode.Space))
        {
            isChargingJump = true;
            jumpForce += Time.deltaTime * jumpForceMultiplier;
            jumpForce = Mathf.Clamp(jumpForce, 0f, maxJumpForce);
        }

        // Отпускаем клавишу Пробел - выполняем прыжок
        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
        {
            Jump();
        }
    }

    void Jump()
    {
        // Получаем направление мыши
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.nearClipPlane;
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 jumpDirection = (targetPosition - transform.position).normalized;

        // Применяем силу рывка в направлении мыши
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.AddForce(jumpDirection * jumpForce, ForceMode2D.Impulse);

        // Задаем ускорение в противоположную сторону для замедления и остановки
        float decelerationForce = -rb.velocity.normalized.magnitude * rb.mass / stoppingDistance;
        rb.AddForce(decelerationForce * rb.velocity.normalized, ForceMode2D.Impulse);

        // Сбрасываем переменные зарядки
        isChargingJump = false;
        jumpForce = 0f;
    }
}
