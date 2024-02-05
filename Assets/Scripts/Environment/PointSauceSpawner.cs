using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointSauceSpawner : MonoBehaviour
{
    public int maxSpawnItems;

    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private float cooldownTime = 5F;

    private Transform[] spawnPoints;
    private static Dictionary<string, bool> sauceSpawnBlockedDictionary = new Dictionary<string, bool>();

    private bool canSpawn = true;


    private void Awake()
    {
        if (itemPrefabs.Length == 0)
            throw new System.Exception($"Не встановлено додного префабу соусу для спавну інпекторі обєкта {transform.gameObject.name}");

        // Вибираємо всі дочірні елементи, крім самого об'єкта
        spawnPoints = GetComponentsInChildren<Transform>(true).Where(child => child != transform).ToArray();
        if (spawnPoints.Length <= 0)
            throw new System.Exception($"Не встановлено точки спавну соуів!! В {transform.gameObject.name} мають лежати елменти itemPoint");

        foreach (var saucePrefab in itemPrefabs)
        {
            sauceSpawnBlockedDictionary[saucePrefab.name] = true;
        }

        StartCoroutine(SpawnItemsRoutine());
    }

    public void BlockSauceSpawn(string sauceName)
    {
        sauceSpawnBlockedDictionary[sauceName] = true;
    }

    public void UnlockSauceSpawn(string sauceName)
    {
        sauceSpawnBlockedDictionary[sauceName] = false;
    }


    private IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);

            if (canSpawn && getCountItems() < maxSpawnItems)
            {
                int randomIndex = Random.Range(0, itemPrefabs.Length);
                var randomSaucePrefab = itemPrefabs[randomIndex];

                if (!sauceSpawnBlockedDictionary[randomSaucePrefab.name])
                {
                    int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
                    Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];

                    if (randomSpawnPoint.childCount < 1)
                    {
                        SpawnItem(randomSaucePrefab, randomSpawnPoint);
                        canSpawn = false;
                        StartCoroutine(CooldownRoutine(randomSaucePrefab));
                    }
                }
            }
        }
    }

    // Повертає кількість соусів на сцені
    private int getCountItems()
    {
        return spawnPoints.Count(spawnPoint => spawnPoint.childCount > 0);
    }

    // Заспавнити предмет
    private void SpawnItem(GameObject spawnItem, Transform spawnPoint)
    {
        GameObject newObject = Instantiate(spawnItem, spawnPoint.position, Quaternion.identity);
        newObject.transform.parent = spawnPoint;
    }

    // Перезарядка на спавн
    private IEnumerator CooldownRoutine(GameObject spawnItem)
    {
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }
}
