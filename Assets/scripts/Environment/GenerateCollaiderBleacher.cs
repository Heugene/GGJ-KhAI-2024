using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class Gene : MonoBehaviour
{
    EdgeCollider2D edgeCollider;
    float _thickness;
    float _radius;
    float radiusCollaider;

    void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        int _numOfCorners = (mesh.vertices.Length - 2) / 2;
        _thickness = mesh.vertices[_numOfCorners].y - mesh.vertices[0].y;
        _radius = mesh.vertices[0].y;
        radiusCollaider = _thickness / 2;
        float _sector = (Mathf.PI * 2) / _numOfCorners;
        float _scaleY = 1f / GetComponentInParent<Transform>().lossyScale.y; 
        Vector2[] _edgeCollaiderVertices = new Vector2[_numOfCorners + 1];

        

        for (int i = 0; i < _numOfCorners; i++)
        {
            _edgeCollaiderVertices[i] = new Vector2(Mathf.Sin(_sector * i) * (radiusCollaider + _radius), Mathf.Cos(_sector * i) * (radiusCollaider * _scaleY + _radius));
        }
        // Закрывающая вершина
        _edgeCollaiderVertices[_numOfCorners] = new Vector2(Mathf.Sin(_sector * _numOfCorners) * (radiusCollaider + _radius), Mathf.Cos(_sector * _numOfCorners) * (radiusCollaider * _scaleY + _radius));

        //Передача вершин в коллайдер
        edgeCollider.points = _edgeCollaiderVertices;
        // Установка толщины EdgeCollider2D
        edgeCollider.edgeRadius = radiusCollaider;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
