using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class GenerateArea : MonoBehaviour
{
    [SerializeField]
    private int _numOfCorners = 18; // Количество углов
    [SerializeField]
    private float _radius = 5.0f; // Радиус многоугольника
    [SerializeField]
    [Header("Удалить предыдущий меш?")]
    private bool _delete = false;
    [SerializeField]
    [Header("Сохранить сгенерируемый меш?")]
    private bool _save = false;

    public int numOfCornes
    {
        get => _numOfCorners;
        private set
        {
            if (value > 2)
            {
                _numOfCorners = value;
            }
        }
    }

    public float radius
    {
        get => _radius;
        private set
        {
            if (value > 0)
            {
                _radius = value;
            }
        }
    }

    public bool save
    {
        get => _save;
        private set { }
    }
    
    public bool delete
    {
        get => _delete;
        private set { }
    }

    private Vector3[] _vertices; // Массив вершин
    private Mesh _mesh; // Полигональная сетка

    void Start()
    {
        // Удаление предыдущего меша по флагу
        if (_delete)
        {
            DeleteMesh();
        }

        // Сгенерировать и сохранить меш если преустановленный пустой 
        if (GetComponent<MeshFilter>().sharedMesh == null) {
            Debug.Log("Mesh Arena generated");
            Generate();
            GenerateUV();
            // Сохранить сгенерированный меш, если установлен соотвествующий флаг
            if (save) {
                SaveMesh();
                Debug.Log("Mesh Arena saved");
            }
        }        
    }

    void Update()
    {
        
    }

    void Generate()
    {
        _mesh = new Mesh();
        _vertices = new Vector3[_numOfCorners];

        GetComponent<MeshFilter>().mesh = _mesh;

        // Вычисление угла между двумя точками относительно центра
        float _sector = Mathf.PI * 2 / _numOfCorners;

        // Расстановка вершин по окружности
        for (int i = 0; i < _numOfCorners; i++)
        {
            _vertices[i] = new Vector3(Mathf.Sin(_sector * i) * _radius, Mathf.Cos(_sector * i) * _radius, 1f);
        }
        // Передача сгенерированных вершин в меш
        _mesh.vertices = _vertices;

        // Обьявление полигонов.
        // Индекс вершины - индекс вершины в массиве Mesh.vertices
        // Один полигон состоит из 3 индексов вершин.
        // У полигона может быть лицевая сторона не там где нужно из-за неправильной очередности вершин
        int[] _triangles = new int[(_numOfCorners - 2) * 3];
        for (int i = 0, j = 0; i < _triangles.Length; i += 3, j++)
        {
            _triangles[i] = 0;
            _triangles[i + 1] = j + 1;
            _triangles[i + 2] = j + 2;
        };

        _mesh.triangles = _triangles;
        //пересчет нормалей. Не факт что будет работать без этого
        _mesh.RecalculateNormals();
    }

    void GenerateUV()
    {
        Vector2[] MeshUV = new Vector2[_vertices.Length];
        
        for(int i = 0; i < _vertices.Length; i++)
        {
            MeshUV[i] = new Vector2(_vertices[i].x / _radius, _vertices[i].y / _radius);
        }
        GetComponent<MeshFilter>().sharedMesh.uv = MeshUV;
    }
    
    // Сохранение меша !!Только во время разработки!!
    void SaveMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        AssetDatabase.CreateAsset(mesh, "Assets/Meshes/AreaMesh.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    void DeleteMesh()
    {
        AssetDatabase.DeleteAsset($"Assets/Meshes/AreaMesh.asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
