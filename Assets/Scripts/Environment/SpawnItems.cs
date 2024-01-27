using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: ���������� �� ������ ��� ����������
public class SpawnItems : MonoBehaviour
{
    public Transform[] itemSpawnPoints;
    public GameObject spawnItem;
    public float spawnTime = 4F;

    // ������ ��� ������������ �������������� ����� ������
    private List<Transform> usedSpawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItemIfNotPresent());
    }

    private IEnumerator SpawnItemIfNotPresent()
    {
        while (true) // ����������� ����, ����� �������� �������� ���������
        {
            foreach (Transform spawnPoint in itemSpawnPoints)
            {
                // ��������, �������������� �� ��� ��� ����� ������
                if (!usedSpawnPoints.Contains(spawnPoint))
                {
                    // �������� ������� ������� � ��������� ����� � �������
                    bool isObjectNearby = Physics.CheckSphere(spawnPoint.position, 0.2F, LayerMask.GetMask("sause"));

                    // Check if there are no colliders (no objects) at the spawn point
                    if (!isObjectNearby)
                    {
                        Instantiate(spawnItem, spawnPoint.position, spawnPoint.rotation);
                        usedSpawnPoints.Add(spawnPoint); // ��������� ����� � ������ ��������������
                    }
                }
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
