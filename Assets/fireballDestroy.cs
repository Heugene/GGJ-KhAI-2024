using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class fireballDestroy : MonoBehaviour
{
    [SerializeField] private float TimeForDestroy = 2f;
    [SerializeField] private PlayerHealthController player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealthController>();
    }

    private void Update()
    {
        StartCoroutine(DeleteObjects());
    }

    IEnumerator DeleteObjects()
    {
        yield return new WaitForSeconds(TimeForDestroy);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            player.TakeDamage(1);
            Destroy(gameObject);
        }

        if (collision.transform.tag == "obstacles")
        {
            Destroy(gameObject);
        }
    }
}
