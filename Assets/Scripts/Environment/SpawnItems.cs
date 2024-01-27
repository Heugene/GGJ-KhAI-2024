using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Переделать на скрипт для мувпоинтов
public class SpawnItems : MonoBehaviour
{
    public Transform[] itemSpawnPoints;
    public GameObject spawnItem;
    public float spawnTime = 4F;

    // Список для отслеживания использованных точек спавна
    private List<Transform> usedSpawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnItemIfNotPresent());
    }

    private IEnumerator SpawnItemIfNotPresent()
    {
        while (true) // бесконечный цикл, чтобы корутина работала постоянно
        {
            foreach (Transform spawnPoint in itemSpawnPoints)
            {
                // Проверка, использовалась ли уже эта точка спавна
                if (!usedSpawnPoints.Contains(spawnPoint))
                {
                    // Проверка наличия объекта с указанным тегом в радиусе
                    bool isObjectNearby = Physics.CheckSphere(spawnPoint.position, 0.2F, LayerMask.GetMask("sause"));

                    // Check if there are no colliders (no objects) at the spawn point
                    if (!isObjectNearby)
                    {
                        Instantiate(spawnItem, spawnPoint.position, spawnPoint.rotation);
                        usedSpawnPoints.Add(spawnPoint); // Добавляем точку в список использованных
                    }
                }
            }

            yield return new WaitForSeconds(spawnTime);
        }
    }
}
