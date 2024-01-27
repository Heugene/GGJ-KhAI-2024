using UnityEngine;

public class ChangeOrderLayer : MonoBehaviour
{
    public BoxCollider2D topCollider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         
        spriteRenderer.sortingOrder = 2;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        spriteRenderer.sortingOrder = 0;
    }
}
