using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LineCollaider : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    byte _flag;
    float _distant;
    Vector3 _startPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject;
            StartPlayerDash();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            EndPlayerDash();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

        }
    }

    void StartPlayerDash()
    {
        _startPoint = _player.transform.position; 
    }

    void EndPlayerDash()
    {
        _distant = Vector3.Distance(_startPoint, _player.transform.position);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
