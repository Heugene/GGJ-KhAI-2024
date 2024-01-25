using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StunEnemy : MonoBehaviour
{
    [SerializeField]
    float TimeOfStun = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy" && collision.transform.GetComponents<BoxCollider2D>().Where(bc => bc.isTrigger == true).First())
        {
            collision.transform.GetComponent<EnemyClown>().enabled = false;
            StartCoroutine(StunEnemyForTime(collision));
        }
    }

    private IEnumerator StunEnemyForTime(Collider2D collision)
    {
        yield return new WaitForSeconds(TimeOfStun);
        collision.transform.GetComponent<EnemyClown>().enabled = true;
        Destroy(gameObject);
    }
}
