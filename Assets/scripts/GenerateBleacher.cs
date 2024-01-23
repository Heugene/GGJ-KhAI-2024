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
    private float _thickness = 4f; //толщина трибуны
    [SerializeField]
    private GenerateArea _arena;

    private Mesh _bleacherMesh;
    private Vector2[] _edgeCollaiderVertices; 
    private float _radius; //Радиус
    private float _sector; // Градус угла (Точка N, центр, Точка N+1)
    private int _numOfCorners; //Количество сторон
    private Vector3[] _vertices; //Вершины сетки трибун. Внутренние перечиляются до индекса _numOfCorners, внешние после него



    void Start()
    {
        // Удаление предыдущего меша по флагу
        if (_arena.delete)
        {
            DeleteMesh("Assets/Meshes/BleacherMesh.asset");
        }

        //Генерация меша, если нет предустановленного
        if (GetComponent<MeshFilter>().sharedMesh == null)
        {
            Generate();
            //Сохранение мешов, если будет сохраняться меш арены
            if (_arena.GetComponent<GenerateArea>().save)
            {
                SaveMesh(GetComponent<MeshFilter>().sharedMesh, "Assets/Meshes/BleacherMesh.asset");
            }
        }
        else {
            //Подгрузка переменных для GenerateEdgeCollaider
            Mesh mesh= GetComponent<MeshFilter>().sharedMesh;
            _radius = mesh.vertices[0].y;
            _numOfCorners = mesh.vertices.Length / 2;
            _sector = (Mathf.PI * 2) / _numOfCorners;
        }
        GenerateEdgeCollaider();
    }

    void Update()
    {
        
    }

    // Генерация меша
    void Generate()
    {
        _radius = _arena.GetComponent<GenerateArea>().radius - 1; // Радиус внутреннего колица вершин
        _numOfCorners = _arena.GetComponent<GenerateArea>().numOfCornes * 4; // Умножаем на 4 чтобы трибуны не были рваными
        _vertices = new Vector3[_numOfCorners*2]; // Умножаем на 2 так как будет две окружности из вершин
        _bleacherMesh = new Mesh();
        GetComponent<MeshFilter>().sharedMesh = _bleacherMesh;

        _sector = (Mathf.PI * 2) / _numOfCorners; // Вычисление угла поворота между точками ( угол (точка N, центр, точка N+1) )

        // Создание двух окружностей из вершин. Расстояние между ними - _thickness
        for (int i = 0; i < _numOfCorners; i++)
        {
            // Генерация внутренних вершин
            _vertices[i] = new Vector3(                Mathf.Sin(_sector * i) * _radius,                Mathf.Cos(_sector * i) * _radius,                0f);
            // Генерация внешних вершин 
            _vertices[i + _numOfCorners] = new Vector3(Mathf.Sin(_sector * i) * (_radius + _thickness), Mathf.Cos(_sector * i) * (_radius + _thickness), 0f);
        };

        // Загрузка сгенерированных вершин в меш
        _bleacherMesh.vertices = _vertices;

        // Обьявление полигонов.
        // Индекс вершины - индекс вершины в массиве Mesh.vertices
        // Один полигон состоит из 3 индексов вершин.
        // У полигона может быть лицевая сторона не там где нужно из-за неправильной очередности вершин
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
        // Дорисовка 2 полигонов вручную, ибо через цикл не сделать
        {
            _triangles[_numOfCorners * 3 * 2] = _numOfCorners * 2 - 1;
            _triangles[_numOfCorners * 3 * 2 + 1] = 0;
            _triangles[_numOfCorners * 3 * 2 + 2] = _numOfCorners - 1;
        }
        {
            _triangles[_numOfCorners * 3 * 2 + 3] = _numOfCorners;
            _triangles[_numOfCorners * 3 * 2 + 4] = 0;
            _triangles[_numOfCorners * 3 * 2 + 5] = _numOfCorners * 2 - 1;
        }
        //

        //Передача массива полигонов в меш
        _bleacherMesh.triangles = _triangles;
        //пересчет нормалей. Не факт что будет работать без этого
        _bleacherMesh.RecalculateNormals();
    }

    // Генерация коллайдера на основе внутренних вершин меша трибуны
    void GenerateEdgeCollaider()
    {
        EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D>();
        float radiusCollaider = _thickness / 2;
        _edgeCollaiderVertices = new Vector2[_numOfCorners + 1];

        for(int i = 0; i < _numOfCorners; i++)
        {
            _edgeCollaiderVertices[i] =         new Vector2(Mathf.Sin(_sector * i) * (radiusCollaider + _radius),                         Mathf.Cos(_sector * i) * (radiusCollaider + _radius));
        }
        // Закрывающая вершина
        _edgeCollaiderVertices[_numOfCorners] = new Vector2(Mathf.Sin(_sector * _numOfCorners) * (radiusCollaider + _radius), Mathf.Cos(_sector * _numOfCorners) * (radiusCollaider + _radius));

        //Передача вершин в коллайдер
        edgeCollider.points = _edgeCollaiderVertices;
        // Установка толщины EdgeCollider2D
        edgeCollider.edgeRadius = radiusCollaider;
    }

    // Сохранение меша !!Только во время разработки!!
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
