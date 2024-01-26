using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;

public class PentagramLogic : MonoBehaviour
{
    [SerializeField]
    float _radiusPoint = 1f;
    [SerializeField]
    float _radiusLine = 5f;
    [SerializeField]
    float _width = 1f;
    [SerializeField]
    GameObject _prefabPaintPoint;
    
    GameObject[] _paintPoints;
    GameObject[] _lineCollaiders;
    float _sector;

    void Start()
    {
        gameObject.transform.position = GetComponentInParent<Transform>().position;
        _prefabPaintPoint = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PaintPoint.prefab");

        _paintPoints = CreatePoints();
        _lineCollaiders = CreateLineCollaider();
    }

    void Update()
    {
        
    }

    Vector3[] CalculatePointsPentagram()
    {
        Vector3[] positionPoints = new Vector3[5];
        float radius = 10f; 
        _sector = (Mathf.PI * 2f) / 5f;

        for (int i = 0; i < 5 ; i++)
        {
            positionPoints[i] = 
                new Vector3(Mathf.Sin(_sector * (float)i) * radius + gameObject.transform.position.x, Mathf.Cos(_sector * i) * radius + gameObject.transform.position.y, gameObject.transform.position.z);
        }
        return positionPoints;
    }

    GameObject[] CreatePoints()
    {
        Vector3[] positionPoints = CalculatePointsPentagram();
        GameObject[] paintPoints = new GameObject[5];
        GameObject objectPaintPoint = new GameObject("PaintPoints");
        objectPaintPoint.transform.parent = transform;

        for (int i = 0; i < 5; i++)
        {
            paintPoints[i] = GameObject.Instantiate<GameObject>(_prefabPaintPoint, positionPoints[i], transform.rotation);
            paintPoints[i].name = $"PaintPoint {i}";
            paintPoints[i].transform.parent = objectPaintPoint.transform;
            paintPoints[i].GetComponent<CircleCollider2D>().radius = _radiusPoint;
            paintPoints[i].SetActive(false);
        }

        return _paintPoints;
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
            EdgeCollider2D EC2D = linesCollaiders[i].AddComponent<EdgeCollider2D>();
            Vector2[] pointsLineCollaider = new Vector2[] { new Vector2(positionPoints[i].x, positionPoints[i].y),
                                            new Vector2(positionPoints[(i+3) % 5].x, positionPoints[(i+3) % 5].y)};
            EC2D.points = pointsLineCollaider;
            EC2D.edgeRadius = _width;
            EC2D.isTrigger = true;

            linesCollaiders[i].SetActive(false);
        }

        return linesCollaiders;
    }


    void Activation()
    {
        _paintPoints[0].SetActive(true);
    }

    void NextPath()
    {
        int index = 1;
        int indexMod = index % _paintPoints.Length;
        _paintPoints[indexMod - 1].SetActive(false);
        _lineCollaiders[indexMod - 1].SetActive(true);
        _paintPoints[indexMod].SetActive(true);

        if (index > _paintPoints.Length)
        {
            Ending();
        }

        index++;
    }

    void Ending()
    {

    }
}
