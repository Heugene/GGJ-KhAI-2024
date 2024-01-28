using System;
using System.Collections.Generic;
using TMPro;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class LineCollaider : MonoBehaviour
{
    [SerializeField]
    GameObject _player;
    [SerializeField]
    [Range(0, 100)]
    public int needDistant = 1;

    bool _playerDashingInCollaider;
    float _distant;
    float _lenght;
    Vector3 _lastPoint;
    PlayerDash _playerDash;
    public GameObject _paintPoint;
    internal GameObject _prefabPaintPoint;

    private void Start()
    {
        EdgeCollider2D ED2D = GetComponent<EdgeCollider2D>();
        _lenght = Vector2.Distance(ED2D.points[0], ED2D.points[1]);
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerDash = _player.GetComponent<PlayerDash>();
        CreatePoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (_playerDash.isDashing)
            {
                _playerDashingInCollaider = true;
            }
            else _playerDashingInCollaider = false;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _playerDash.isDashing)
        {
            _playerDashingInCollaider = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _distant += Vector3.Distance(_player.transform.position, _lastPoint);
            _lastPoint = _player.transform.position;
            if (_playerDash == true)
            {
                if (_playerDashingInCollaider == false)
                {
                    _playerDashingInCollaider = true;
                }
                else
                {
                    _distant += Vector3.Distance(_player.transform.position, _lastPoint);
                }

            }
            if (_playerDash == false && _playerDashingInCollaider == true)
            {
                _playerDashingInCollaider = false;
            }
            if (_distant >= _lenght * (needDistant / 100f))
            {
                DistanceComplete();
            }
        }
    }

    void PathEnd()
    {
        LineComplete?.Invoke(gameObject);
        gameObject.SetActive(false);
    }

    void StartPlayerDash()
    {
        _playerDashingInCollaider = true;

    }

    void EndPlayerDash()
    {
        _playerDashingInCollaider = false;
    }

    void CreatePoint()
    {
        Vector2[] _pointsCoord = GetComponent<EdgeCollider2D>().points;

        _paintPoint = GameObject.Instantiate(_prefabPaintPoint);
        _paintPoint.name = "PaintPoint";
        _paintPoint.tag = "PaintPoint";
        _paintPoint.transform.parent = gameObject.transform;
        _paintPoint.transform.position = gameObject.GetComponent<EdgeCollider2D>().points[1];
        _paintPoint.GetComponent<CircleCollider2D>().radius = GetComponentInParent<PentagramLogic>()._radiusPointAndLine;

        _paintPoint.GetComponent<PaintPoint>().IsPushed += PathEnd;
        DistanceComplete += _paintPoint.GetComponent<PaintPoint>().DistanceComplete;
    }

    public delegate void LineCompleted(GameObject ths);
    internal event LineCompleted LineComplete;

    internal event Action DistanceComplete;
}