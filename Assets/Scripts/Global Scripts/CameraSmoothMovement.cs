using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class cameraSmoothMovement : MonoBehaviour
{
    [SerializeField]
    GameObject PlayerTransform;
    [SerializeField]
    private float speed = 1f;
    void Start()
    {
        PlayerTransform = GameObject.FindGameObjectsWithTag("Player").First();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = new Vector3(PlayerTransform.transform.position.x, PlayerTransform.transform.position.y, -8f);
        transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
