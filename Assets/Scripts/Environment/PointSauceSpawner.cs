using System.Collections;
using System.Linq;
using UnityEngine;

public class PointSauceSpawner : MonoBehaviour
{
    public GameObject itemPrefab;
    private Transform[] spawnPoints;
    public int maxSpawnItems = 1;
    public float cooldownTime = 5F;

    private bool canSpawn = true;


    private void Start()
    {
        if (itemPrefab == null)
            throw new System.Exception($"Не встановлено префаб для спавну інпекторі обєкта {transform.gameObject.name}");

        // Вибираємо всі дочірні елементи, крім самого об'єкта
        spawnPoints = GetComponentsInChildren<Transform>(true).Where(child => child != transform).ToArray();
        if (spawnPoints.Length <= 0)
            throw new System.Exception($"Не встановлено точки спавну соуів!! В {transform.gameObject.name} мають лежати елменти itemPoint");
        StartCoroutine(SpawnItemsRoutine());
    }

    private IEnumerator SpawnItemsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(cooldownTime);

            if (canSpawn && getCountItems() < maxSpawnItems)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                Transform randomSpawnPoint = spawnPoints[randomIndex];

                if (randomSpawnPoint.childCount < 1)
                {
                    SpawnItem(itemPrefab, randomSpawnPoint);
                    canSpawn = false;
                    StartCoroutine(CooldownRoutine(itemPrefab));
                }
            }
        }
    }

    private int getCountItems()
    {
        return spawnPoints.Count(spawnPoint => spawnPoint.childCount > 0);
    }

    private void SpawnItem(GameObject spawnItem, Transform spawnPoint)
    {
        GameObject newObject = Instantiate(spawnItem, spawnPoint.position, Quaternion.identity);
        newObject.transform.parent = spawnPoint;
    }

    private IEnumerator CooldownRoutine(GameObject spawnItem)
    {
        yield return new WaitForSeconds(cooldownTime);
        canSpawn = true;
    }
}
