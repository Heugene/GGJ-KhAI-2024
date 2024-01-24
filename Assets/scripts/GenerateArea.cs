using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class GenerateArea : MonoBehaviour
{
    [SerializeField]
    private int _numOfCorners = 18; // ���������� �����
    [SerializeField]
    private float _radius = 5.0f; // ������ ��������������
    [SerializeField]
    [Header("������� ���������� ���?")]
    private bool _delete = false;
    [SerializeField]
    [Header("��������� ������������� ���?")]
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

    private Vector3[] _vertices; // ������ ������
    private Mesh _mesh; // ������������� �����

    void Start()
    {
        // �������� ����������� ���� �� �����
        if (_delete)
        {
            DeleteMesh();
        }

        // ������������� � ��������� ��� ���� ���������������� ������ 
        if (GetComponent<MeshFilter>().sharedMesh == null) {
            Debug.Log("Mesh Arena generated");
            Generate();
            GenerateUV();
            // ��������� ��������������� ���, ���� ���������� �������������� ����
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

        // ���������� ���� ����� ����� ������� ������������ ������
        float _sector = Mathf.PI * 2 / _numOfCorners;

        // ����������� ������ �� ����������
        for (int i = 0; i < _numOfCorners; i++)
        {
            _vertices[i] = new Vector3(Mathf.Sin(_sector * i) * _radius, Mathf.Cos(_sector * i) * _radius, 1f);
        }
        // �������� ��������������� ������ � ���
        _mesh.vertices = _vertices;

        // ���������� ���������.
        // ������ ������� - ������ ������� � ������� Mesh.vertices
        // ���� ������� ������� �� 3 �������� ������.
        // � �������� ����� ���� ������� ������� �� ��� ��� ����� ��-�� ������������ ����������� ������
        int[] _triangles = new int[(_numOfCorners - 2) * 3];
        for (int i = 0, j = 0; i < _triangles.Length; i += 3, j++)
        {
            _triangles[i] = 0;
            _triangles[i + 1] = j + 1;
            _triangles[i + 2] = j + 2;
        };

        _mesh.triangles = _triangles;
        //�������� ��������. �� ���� ��� ����� �������� ��� �����
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
    
    // ���������� ���� !!������ �� ����� ����������!!
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
