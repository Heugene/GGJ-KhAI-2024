using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class StunEnemy : MonoBehaviour
{
    [SerializeField]
    float TimeOfStun = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool objectDetected = collision.transform.GetComponents<BoxCollider2D>().Where(bc => bc.isTrigger == true).FirstOrDefault();

        if (collision.transform.tag == "Enemy" && objectDetected)
        {
            collision.transform.GetComponent<EnemyClown>().enabled = false;
            collision.transform.GetComponent<NavMeshAgent>().enabled = false;
            StartCoroutine(StunEnemyForTime(collision));
        }
    }

    private IEnumerator StunEnemyForTime(Collider2D collision)
    {
        yield return new WaitForSeconds(TimeOfStun);
        collision.transform.GetComponent<EnemyClown>().enabled = true;
        collision.transform.GetComponent<NavMeshAgent>().enabled = true;
        Destroy(gameObject);
    }
}
