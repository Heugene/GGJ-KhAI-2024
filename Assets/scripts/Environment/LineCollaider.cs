using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LineCollaider : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    [Range(0, 100)]
    public int needDistant = 1;

    bool _flag;
    float _distant;
    float _lenght;
    Vector3 _startPoint;
    PlayerDash _playerDash;
    public GameObject _paintPoint;

    private void Start()
    {
        EdgeCollider2D ED2D = GetComponent<EdgeCollider2D>();
        _lenght = Vector2.Distance(ED2D.points[0], ED2D.points[1]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _player = collision.gameObject;
            _playerDash = _player.GetComponent<PlayerDash>();

            if (_playerDash.isDashing)
            {
                StartPlayerDash();
            }
            else _flag = false;

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
            if (_playerDash == true && _flag == false)
            {
                StartPlayerDash();
            }

            if (_playerDash == false && _flag == true)
            {
                EndPlayerDash();
            }
            if (_distant >= _lenght * (needDistant / 100f))
            {
                _paintPoint.GetComponent<PaintPoint>().DistanceComplete();
            }
        }
    }

    void StartPlayerDash()
    {
        _flag = true;
        _startPoint = _player.transform.position; 
    }

    void EndPlayerDash()
    {
        _flag = false;
        _distant += Vector3.Distance(_startPoint, _player.transform.position);
    }
}
