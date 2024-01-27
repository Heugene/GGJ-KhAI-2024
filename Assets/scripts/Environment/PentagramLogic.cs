using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System;
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
    float _width = 2f;
    [SerializeField]
    GameObject _prefabPaintPoint;
    
    GameObject[] _paintPoints;
    GameObject[] _lineCollaiders;
    float _sector;
    int index = 1;

    void Start()
    {
        gameObject.transform.position = GetComponentInParent<Transform>().position;
        _prefabPaintPoint = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/PaintPoint.prefab");

        _paintPoints = CreatePoints();
        _lineCollaiders = CreateLineCollaider();

        Activation();
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
            int indexMod = ((i - 3) + 3) % 5;
            paintPoints[indexMod] = GameObject.Instantiate<GameObject>(_prefabPaintPoint, positionPoints[indexMod], transform.rotation);
            paintPoints[indexMod].name = $"PaintPoint {indexMod}";
            paintPoints[indexMod].tag = "PaintPoint";
            paintPoints[indexMod].transform.parent = objectPaintPoint.transform;
            paintPoints[indexMod].GetComponent<CircleCollider2D>().radius = _radiusPoint;
            paintPoints[indexMod].SetActive(false);
        }

        return paintPoints;
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
            linesCollaiders[i].AddComponent<LineCollaider>();
            linesCollaiders[i].GetComponent<LineCollaider>()._paintPoint = _paintPoints[(i + 3)%5];
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


    public void Activation()
    {
        _paintPoints[0].SetActive(true);
        _paintPoints[0].GetComponent<PaintPoint>().DistanceComplete();
    }

    public void NextPath()
    {
        _paintPoints[index-1].SetActive(false);
        try
        {
            _lineCollaiders[index - 1].SetActive(false);
        }
        catch (Exception e)
        {
            Debug.LogError($"[{e.Message}] Все идет по плану");
        }
        _lineCollaiders[index].SetActive(true);
        _paintPoints[(index+2) % 5].SetActive(true);

        if (index > _paintPoints.Length)
        {
            Ending();
        }

        index++;
    }

    void Ending()
    {
        Debug.Log("Все хуйня, давай по новой");
    }
}
