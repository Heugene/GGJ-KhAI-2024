using System.Collections;
using UnityEngine;

public class fireballDestroy : MonoBehaviour
{
    [SerializeField] private float TimeForDestroy = 2f;

    private PlayerHealthController player;
    private int clownDamage;


    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealthController>();
        clownDamage = GameObject.FindWithTag("Enemy").GetComponent<EnemyClown>().clownDamage;
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
            player.TakeDamage(clownDamage);
            Destroy(gameObject);
        }

        if (collision.transform.tag == "obstacles")
        {
            Destroy(gameObject);
        }
    }
}
