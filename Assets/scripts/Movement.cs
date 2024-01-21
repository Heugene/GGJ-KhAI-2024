using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float maxJumpDistance = 5f;
    [SerializeField]
    private float chargedJumpDistance = 0;
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.05f;

    private Vector2 mousePosition;
    private Vector2 LastMousePosition;
    private bool IsMoving = false;
    private bool IsSpacePressed = false;

    void FixedUpdate()
    {
        // ѕолучаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        if (!IsMoving && IsSpacePressed)
        {
            if (maxJumpDistance > chargedJumpDistance)
                chargedJumpDistance += chargedJumpDistanceMultiplier;
            else
                chargedJumpDistance = maxJumpDistance;
        }

        if (IsMoving)
        {
            // ѕеремещаем персонаж
            MoveToTarget(LastMousePosition);
        }
    }

    void Update()
    {
        // ѕолучаем позицию мыши в мировых координатах
        mousePosition = GetMouseWorldPosition();

        if (Input.GetKey(KeyCode.Space))
        {
            IsMoving = false; 
            IsSpacePressed = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsMoving = true;
            IsSpacePressed = false;
            // ¬ычисл€ем вектор направлени€ от текущей позиции до позиции мыши
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // ќграничиваем длину вектора до maxJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }
    }

    void MoveToTarget(Vector2 LastMousePosition)
    {
        // ѕримен€ем перемещение вдоль вектора направлени€ с посто€нной скоростью
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector2 mousePosition = Input.mousePosition;
        return Camera.main.ScreenToWorldPoint(mousePosition);
    }
}
