using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]

public class GenerateBleacher : MonoBehaviour
{
    [SerializeField]
    private float _thickness = 4f; //толщина трибуны
    [SerializeField]
    private GameObject _arena; // Поместить сюда ссылку на арену для передачи радиуса и количество вершин

    private Mesh _bleacherMesh;
    private Mesh _collaiderMesh; 
    private float _radius; //Радиус
    private int _numOfCorners; //Количество сторон
    private Vector3[] _vertices; //Вершины сетки трибун. Внутренние перечиляются до индекса _numOfCorners, внешние после него



    void Start()
    {
        //Генерация меша, если нет предустановленного
        if (GetComponent<MeshCollider>().sharedMesh == null)
        {
            Generate();
            GenerateCollaiderMesh();

            //Сохранение меша, если будет сохраняться меш арены
            if (_arena.GetComponent<GenerateArea>().save)
            {
                SaveMesh(GetComponent<MeshFilter>().sharedMesh, "Assets/Meshes/BleacherMesh.asset");
                SaveMesh(GetComponent<MeshCollider>().sharedMesh, "Assets/Meshes/BleacherCollaiderMesh.asset");
            }
        }
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

        float _sector = (Mathf.PI * 2) / _numOfCorners; // Вычисление угла поворота между точками ( угол (точка N, центр, точка N+1) )

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

    void GenerateCollaiderMesh()
    {
        _collaiderMesh = new Mesh();
        GetComponent<MeshCollider>().sharedMesh = _collaiderMesh;
        Vector3[] _verticesCollaiderMesh = new Vector3[_numOfCorners*2];
        // Копируем внутренние вершины
        Array.Copy(_vertices, _verticesCollaiderMesh, _numOfCorners);
        
        //Создаем вершины верхних слоев
        for(int i = 0; i < _numOfCorners; i++) 
        {
            _verticesCollaiderMesh[i + _numOfCorners] = _verticesCollaiderMesh[i] + new Vector3(0, 0, 20);
        }
        //Отодвигаем вершины нижних слоев
        for (int i = 0; i < _numOfCorners; i++)
        {
            _verticesCollaiderMesh[i] = _verticesCollaiderMesh[i] + new Vector3(0, 0, -5);
        }

        _collaiderMesh.vertices = _verticesCollaiderMesh;

        // Обьявление полигонов коллайдера
        // Индекс вершины - индекс вершины в массиве Mesh.vertices
        // Один полигон состоит из 3 индексов вершин.
        // У полигона может быть лицевая сторона не там где нужно из-за неправильной очередности вершин
        int[] _triangles = new int[_numOfCorners * 3 * 2 + 6];
        for (int i = 0, j = 0; i < _numOfCorners * 3 * 2 - 6; i += 6, j++)
        {
            _triangles[i] = j;
            _triangles[i + 1] = _numOfCorners + j + 1;
            _triangles[i + 2] = _numOfCorners + j;
            _triangles[i + 3] = _numOfCorners + j + 1;
            _triangles[i + 4] = j;
            _triangles[i + 5] = j + 1;
        };
        // Дорисовка 2 полигонов вручную, ибо через цикл не сделать
        {
            _triangles[_numOfCorners * 3 * 2] = 0;
            _triangles[_numOfCorners * 3 * 2 + 1] = _numOfCorners * 2 - 1;
            _triangles[_numOfCorners * 3 * 2 + 2] = _numOfCorners - 1;
        }
        {
            _triangles[_numOfCorners * 3 * 2 + 3] = 0;
            _triangles[_numOfCorners * 3 * 2 + 4] = _numOfCorners;
            _triangles[_numOfCorners * 3 * 2 + 5] = _numOfCorners * 2 - 1;
        }
        //

        //Передача массива полигонов в меш
        _collaiderMesh.triangles = _triangles;
        //пересчет нормалей. Не факт что будет работать без этого
        _collaiderMesh.RecalculateNormals();
    }

    // Сохранение меша !!Только во время разработки!!
    void SaveMesh(Mesh mesh, string path)
    {
        AssetDatabase.CreateAsset(mesh, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
