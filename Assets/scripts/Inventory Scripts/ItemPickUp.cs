using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class ItemPickUp : MonoBehaviour
{
    public float PickUpRadius = 1f;
    public SOItems ItemData;
    public AudioClip pickUp;
    AudioSource audioSource;

    private CircleCollider2D myCollider;
    [SerializeField]private PlayerHealthController playerHealthController;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = pickUp;
    }

    private void Awake()
    {
        myCollider = GetComponent<CircleCollider2D>();
        myCollider.isTrigger = true;
        myCollider.radius = PickUpRadius;
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerHealthController = playerObject.GetComponent<PlayerHealthController>();

            if (playerHealthController == null)
            {
                Debug.LogError("Player Health Controller not found on the object with the 'Player' tag!");
            }
        }
        else
        {
            Debug.LogError("Player object not found with the 'Player' tag!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var inventory = other.transform.GetComponent<InventoryHolder>();

        if (!inventory) return;
        audioSource.Play();
        StartCoroutine(onSoundEnd(audioSource.clip.length, inventory));

    }

    IEnumerator onSoundEnd(float seconds, InventoryHolder inventory)
    {
        yield return new WaitForSeconds(seconds);
        if (ItemData.ItemType == ItemType.Cheese)
        {
            playerHealthController.Heal(1);
            Destroy(gameObject);
        }
        else
        {
            if (inventory.InventorySystem.AddToInventory(ItemData, 1))
            {
                Destroy(gameObject);
            }
        }
    }
}
