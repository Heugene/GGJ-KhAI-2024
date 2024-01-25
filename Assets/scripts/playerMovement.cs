using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 7f; // Øâèäê³ñòü ïîëüîòó ó ñòðèáêó
    [SerializeField]
    private float maxJumpDistance = 5f; // Ìàêñèìàëüíà äàëüí³ñòü ñòðèáêó
    private float chargedJumpDistance = 0; // Ïîòî÷íèé çàðÿä ñòðèáêó (Ó åêâ³âàëåíò³ â³äñòàí³)
    [SerializeField]
    private float chargedJumpDistanceMultiplier = 0.4f; // Ïðèð³ñò çàðÿäó
    [SerializeField]
    private float DashSpeed = 12;
    private float DashSpeedTemp = 0;
    [SerializeField]
    private float DashCooldown = 1;
    private float DashCooldownTemp = 0;
    [SerializeField]
    private float DashSpeedReducer = 0.05f;

    private Vector2 mousePosition; // Êîîðäèíàòè ìèø³
    private Vector2 LastMousePosition; // Îñòàíí³ êîîðäèíàòè ìèø³
    public bool IsMoving = false; // Ïðàïîðåöü ñòàíó ðóõó
    public bool IsButtonJumpPressed = false; // Ïðàïîðåöü óòðèìàííÿ êíîïêè ðóõó
    public bool isPlayerHitEnemy = false; // çì³ííà äëÿ âèçíà÷åííÿ ÷è ãðàâåöü ç³òêíóâñÿ ç âîðîãîì
    public bool isCanDash = false; // ìîæåò ëè èãðîê ñäåëàòü äåø
    public bool isDashing = false; // äåëàåò ëè èãðîê äåø


    private void Start()
    {
        DashSpeedTemp = DashSpeed;

        inventoryDisplay = InventoryDisplay.Instance;

        if (inventoryDisplay != null)
        {
            inventoryDisplay.OnCurrentItemChanged += HandleCurrentItemChanged;
        }
        else
        {
            Debug.LogError("InventoryDisplay not found.");
        }
    }
    private void HandleCurrentItemChanged(SOItems newItem)
    {
        if (newItem == null || newItem.ItemType == null)
        {
            currentItemItemType = ItemType.None;
        }
        else
        {
            currentItemItemType = newItem.ItemType;
        }
    }


    // ëîãèêà
    void FixedUpdate()
    {
        // Ïîëó÷àåì ïîçèöèþ ìûøè â ìèðîâûõ êîîðäèíàòàõ
        mousePosition = GetMouseWorldPosition();

        // åñëè ó íàñ åêèïèðîâàí ïðàâèëüíûé ïðåäìåò òî ìû ìîæåì äåëàòü äåø
        if(isCanDash)
            MakeDash();

        if (isDashing)
        {
            CalculateDash();
            CalculateDashReload();
        }

        // ñ÷èòàåò äèñòàíöèþ ïðûæêà
        CalculateJumpDistance();

        if (IsMoving && !isPlayerHitEnemy && isCanDash == false)
            MoveToTarget(LastMousePosition);// ïðûæîê ïåðñîíàæà
    }

    // input ïðîöåñû
    void Update()
    {
        // Ïîëó÷àåì ïîçèöèþ ìûøè â ìèðîâûõ êîîðäèíàòàõ
        mousePosition = GetMouseWorldPosition();

        // Äîäàëè ï³äòðèìêó ËÊÌ
        // ßêùî íàòèñêàºìî êíîïêó ïåðåñóâàííÿ
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
        {
            IsMoving = false;
            IsButtonJumpPressed = true;
        }

        // ßêùî â³äïóñòèëè êíîïêó ïåðåñóâàííÿ
        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0) && isDashing == false)
        {
            isCanDash = false;
            IsMoving = true;
            IsButtonJumpPressed = false;
            isPlayerHitEnemy = false;
            // Âû÷èñëÿåì âåêòîð íàïðàâëåíèÿ îò òåêóùåé ïîçèöèè äî ïîçèöèè ìûøè
            Vector2 direction = (mousePosition - (Vector2)transform.position);

            // Îãðàíè÷èâàåì äëèíó âåêòîðà äî chargedJumpDistance
            LastMousePosition = (Vector2)transform.position + Vector2.ClampMagnitude(direction, chargedJumpDistance);
            chargedJumpDistance = 0;
        }

        // åñëè èãðîê ïîäõîäèò î÷åíü áëèçêî ê ïîçèöèè ïðûæêà òî IsMoving = false (íóæíî äëÿ àíèìàöèé)
        var diraction = (Vector2)transform.position - LastMousePosition;
        if (diraction.magnitude < 0.1)
            IsMoving = false;

        // íà÷àëî äåøà
        if (Input.GetKeyUp(KeyCode.Mouse1) && !isDashing)
        {
            isCanDash = true;
            isDashing = true;
        }
    }

    // çàðÿäêà ïðûæêà
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

    // óìåíüøåíèå ñêîðîñòè äåøà íà çàäàíîå çíà÷åíèå DashSpeedReducer
    void CalculateDash()
    {
        if (isCanDash && isDashing)
        {
            DashSpeed -= DashSpeedReducer;
            if(DashSpeed <= 0)
            {
                isCanDash = false;
                isDashing = false;
                DashSpeed = DashSpeedTemp;
            }
        }
    }

    // ïåðåçàðÿäêà äåøà â ñåêóíäàõ
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

    // Ìåòîä ïåðåñóâàííÿ äî çàäàíî¿ òî÷êè
    void MoveToTarget(Vector2 LastMousePosition)
    {
        // Ïðèìåíÿåì ïåðåìåùåíèå âäîëü âåêòîðà íàïðàâëåíèÿ ñ ïîñòîÿííîé ñêîðîñòüþ
        transform.position = Vector2.MoveTowards(transform.position, LastMousePosition, moveSpeed * Time.deltaTime);
    }

    // Ñîçäàíèå äåøà
    void MakeDash()
    {
        // Âû÷èñëÿåì âåêòîð íàïðàâëåíèÿ îò òåêóùåé ïîçèöèè äî ïîçèöèè ìûøè
        Vector2 direction = mousePosition - (Vector2)transform.position;

        // Ïðèìåíÿåì MoveTowards äëÿ äâèæåíèÿ ê êóðñîðó
        transform.position = Vector2.MoveTowards(transform.position, (Vector2)transform.position + direction.normalized, DashSpeed * Time.deltaTime);

        // Îáíîâëÿåì LastMousePosition
        LastMousePosition = transform.position;
    }

    // Ôóíêö³ÿ, ùî ïîâåðòàº ñâ³òîâ³ êîîðäèíàòè ìèø³
    public Vector2 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    // îñòàíîâêà èãðîêà êîãäà îí ñòàëêèâàåòüñÿ ñ ïðîòèâíèêîì
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