using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class BananaStun : MonoBehaviour
{
    [SerializeField]
    float TimeOfStun = 3f;
    private Animator clownAnimator;
    private ClownAnimator clownComponent;

    private void Start()
    {
        GameObject clownObj = GameObject.FindGameObjectsWithTag("Enemy").FirstOrDefault();
        clownComponent = clownObj.GetComponent<ClownAnimator>();
        clownAnimator = clownObj.GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool objectDetected = collision.transform.GetComponents<BoxCollider2D>().Where(bc => bc.isTrigger == true).FirstOrDefault();

        if (collision.transform.tag == "Enemy" && objectDetected)
        {
            StartCoroutine(StunEnemyForTime(collision));
        }
    }

    private IEnumerator StunEnemyForTime(Collider2D collision)
    {
        clownAnimator.SetBool("isFall", true);
        clownComponent.Freeze(true);

        yield return new WaitForSeconds(TimeOfStun);
        clownAnimator.SetBool("isFall", false);

        yield return new WaitForSeconds(0.5f);
        clownComponent.Freeze(false);

        Destroy(gameObject);
    }
}
