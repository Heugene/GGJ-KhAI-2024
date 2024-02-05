using System;
using System.Drawing;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(EdgeCollider2D))]

public class GenerateBleacher : MonoBehaviour
{
    [SerializeField]
    private float _thickness = 4f; //������� �������
    [SerializeField]
    private GenerateArea _arena;

    private Mesh _bleacherMesh;
    private Vector2[] _edgeCollaiderVertices; 
    private float _radius; //������
    private float _sector; // ������ ���� (����� N, �����, ����� N+1)
    private int _numOfCorners; //���������� ������
    private Vector3[] _vertices; //������� ����� ������. ���������� ������������ �� ������� _numOfCorners, ������� ����� ����



    void Start()
    {
        // �������� ����������� ���� �� �����
        if (_arena.delete)
        {
            DeleteMesh("Assets/Meshes/BleacherMesh.asset");
        }

        //��������� ����, ���� ��� ������������������
        if (GetComponent<MeshFilter>().sharedMesh == null)
        {
            Generate();
            //���������� �����, ���� ����� ����������� ��� �����
            if (_arena.GetComponent<GenerateArea>().save)
            {
                SaveMesh(GetComponent<MeshFilter>().sharedMesh, "Assets/Meshes/BleacherMesh.asset");
            }
        }
        else {
            //��������� ���������� ��� GenerateEdgeCollaider
            Mesh mesh= GetComponent<MeshFilter>().sharedMesh;
            _radius = mesh.vertices[0].y;
            _numOfCorners = mesh.vertices.Length / 2;
            _sector = (Mathf.PI * 2) / _numOfCorners;
        }
    }

    void Update()
    {
        
    }

    // ��������� ����
    void Generate()
    {
        _radius = _arena.GetComponent<GenerateArea>().radius - 1; // ������ ����������� ������ ������
        _numOfCorners = _arena.GetComponent<GenerateArea>().numOfCornes * 4; // �������� �� 4 ����� ������� �� ���� �������
        _vertices = new Vector3[_numOfCorners*2 + 2]; // �������� �� 2 ��� ��� ����� ��� ���������� �� ������
        _bleacherMesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = _bleacherMesh;

        _sector = (Mathf.PI * 2) / _numOfCorners; // ���������� ���� �������� ����� ������� ( ���� (����� N, �����, ����� N+1) )

        // �������� ���� ����������� �� ������. ���������� ����� ���� - _thickness
        for (int i = 0; i < _numOfCorners; i++)
        {
            // ��������� ���������� ������
            _vertices[i] = new Vector3(                Mathf.Sin(_sector * i) * _radius,                Mathf.Cos(_sector * i) * _radius,                0f);
            // ��������� ������� ������ 
            _vertices[i + _numOfCorners] = new Vector3(Mathf.Sin(_sector * i) * (_radius + _thickness), Mathf.Cos(_sector * i) * (_radius + _thickness), 0f);
        };
        {
            _vertices[_numOfCorners*2] =     new Vector3(Mathf.Sin(_sector * 0) * _radius,                Mathf.Cos(_sector * _numOfCorners) * _radius,                0f);
            _vertices[_numOfCorners*2 + 1] = new Vector3(Mathf.Sin(_sector * 0) * (_radius + _thickness), Mathf.Cos(_sector * _numOfCorners) * (_radius + _thickness), 0f);
        }
        

        // �������� ��������������� ������ � ���
        _bleacherMesh.vertices = _vertices;

        // ���������� ���������.
        // ������ ������� - ������ ������� � ������� Mesh.vertices
        // ���� ������� ������� �� 3 �������� ������.
        // � �������� ����� ���� ������� ������� �� ��� ��� ����� ��-�� ������������ ����������� ������
        int[] _triangles = new int[_numOfCorners*3*2+6];
        for (int i = 0, j = 0; i < _numOfCorners*3*2-6; i += 6, j++)
        {
            _triangles[i] = _numOfCorners + j;
            _triangles[i + 1] = _numOfCorners + j + 1;
            _triangles[i + 2] = j;
            _triangles[i + 3] = j;
            _triangles[i + 4] = _numOfCorners + j + 1;
            _triangles[i + 5] = j + 1;
        };
        {
            _triangles[^6] = _numOfCorners * 2;
            _triangles[^5] = _numOfCorners * 2 - 1;
            _triangles[^4] = _numOfCorners * 2 + 1;
        }
        {
            _triangles[^3] = _numOfCorners * 2 - 1;
            _triangles[^2] = _numOfCorners * 2;
            _triangles[^1] = _numOfCorners - 1;
        }

        //�������� ������� ��������� � ���
        _bleacherMesh.triangles = _triangles;

        RecalculateUvMap();
        //�������� ��������. �� ���� ��� ����� �������� ��� �����
        _bleacherMesh.RecalculateNormals();
    }


    void RecalculateUvMap()
    {
        Vector2[] _myUV = new Vector2[_bleacherMesh.vertices.Length];
        float counterIteration = 0;
        float step = 1f / (_vertices.Length);
        
        // ����������� ������ UV
        for (int i = 0; i < (_vertices.Length - 2)/2 ; i++, counterIteration++)
        {  
            // ����������� ������ UV � ������� ����� ��������
            _myUV[i]                              = new Vector2(step * counterIteration, 0);
            // ����������� ������ UV � ������ ����� ��������
            _myUV[i + (_vertices.Length - 2) / 2] = new Vector2(step * counterIteration, 1);
        }

        // ������ ���������� ��������� UV ������ ���� ��� ������ !!��� ��� �����!!
        _myUV[^2] = new Vector2((step * counterIteration), 0);
        _myUV[^1] = new Vector2((step * counterIteration), 1);

        _bleacherMesh.uv = _myUV;
        GetComponent<MeshFilter>().sharedMesh = _bleacherMesh;
    }

    // ���������� ���� !!������ �� ����� ����������!!
    void SaveMesh(Mesh mesh, string path)
    {
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void DeleteMesh(string path)
    {
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
