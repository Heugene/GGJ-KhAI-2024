using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class PentagramLogic : MonoBehaviour
{
    [SerializeField]
    public float _radius = 8f;
    [SerializeField]
    public float _radiusPointAndLine = 2f;
    [SerializeField]
    float _width = 2f;

    GameObject _startPoint;
    GameObject[] _lineCollaiders;
    float _sector;
    int index = 1;

    void Start()
    {
        gameObject.transform.position = GetComponentInParent<Transform>().position;

        _lineCollaiders = CreateLineCollaider();
        _startPoint = CreateStartPoint(_lineCollaiders[0].GetComponent<EdgeCollider2D>().points[0]);
    }

    void Update()
    {
        
    }

    Vector3[] CalculatePointsPentagram()
    {
        Vector3[] positionPoints = new Vector3[5];
        _sector = (Mathf.PI * 2f) / 5f;

        for (int i = 0; i < 5 ; i++)
        {
            positionPoints[i] = 
                new Vector3(Mathf.Sin(_sector * (float)i) * _radius + gameObject.transform.position.x, Mathf.Cos(_sector * i) * _radius + gameObject.transform.position.y, gameObject.transform.position.z);
        }
        return positionPoints;
    }

    GameObject[] CreateLineCollaider()
    {
        Vector3[] positionPoints = CalculatePointsPentagram();
        GameObject[] linesCollaiders = new GameObject[5];
        GameObject objectLinesCollaider = new GameObject("LinesCollaider");
        objectLinesCollaider.transform.parent = transform;

        for (int i = 0; i < 5; i++)
        {

            linesCollaiders[i] = new GameObject($"LineCollaide {i}");
            linesCollaiders[i].transform.parent = objectLinesCollaider.transform;
            LineCollaider _line_collaider = linesCollaiders[i].AddComponent<LineCollaider>();
            EdgeCollider2D EC2D = linesCollaiders[i].AddComponent<EdgeCollider2D>();
            Vector2[] pointsLineCollaider = new Vector2[] { new Vector2(positionPoints[i].x, positionPoints[i].y),
                                            new Vector2(positionPoints[(i+3) % 5].x, positionPoints[(i+3) % 5].y)};
            EC2D.points = pointsLineCollaider;
            EC2D.edgeRadius = _width;
            EC2D.isTrigger = true;

            linesCollaiders[i].GetComponent<LineCollaider>().LineComplete += NextPath;

            linesCollaiders[i].SetActive(false);
        }

        return linesCollaiders;
    }

    GameObject CreateStartPoint(Vector3 _coordStartPoint)
    {
        GameObject prefabPaintPoint = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PaintPoint.prefab");
        GameObject startPaintPoint = GameObject.Instantiate(prefabPaintPoint);
        startPaintPoint.name = "Start PaintPoint";
        startPaintPoint.transform.position = _coordStartPoint;
        startPaintPoint.GetComponent<CircleCollider2D>().radius = _radiusPointAndLine;
        startPaintPoint.GetComponent<PaintPoint>().DistanceComplete();
        startPaintPoint.GetComponent<PaintPoint>().IsPushed += StartPath;
        startPaintPoint.SetActive(false);

        return startPaintPoint;
    }

    public void Activation()
    {
        _startPoint.SetActive(true);
    }

    public void StartPath()
    {
        _lineCollaiders[0].SetActive(true);
    }

    public void NextPath(GameObject _lineCollaider)
    {
        if (index == 5) 
        {
            Debug.Log("Пентаграма зарахована");
            End?.Invoke();
        }

        GameObject[] next =
            (from lineCollaider in _lineCollaiders
             where (lineCollaider.GetComponent<EdgeCollider2D>().points[0] == _lineCollaider.GetComponent<EdgeCollider2D>().points[1])
             select lineCollaider).ToArray();
        next[0].SetActive(true);

        index++;
    }

    internal event Action End;
}
