using UnityEngine;

public class RandomSauceSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn1;
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;
    public int numberOfPrefabs = 2;
    public Vector2 spawnAreaSize = new Vector2(35f, 35f);
    public float randomScale = 0.5f; // Уменьшенный коэффициент разброса

    void Start()
    {

    }
    public void SpawnKetchup()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector2 randomPosition = GetRandomPosition();

            GameObject spawnedObject = Instantiate(prefabToSpawn1, (Vector2)transform.position + randomPosition, Quaternion.identity);
            spawnedObject.transform.parent = transform; // Установка текущего объекта в качестве родительского
        }
    }   
    public void SpawnMayonnaise()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector2 randomPosition = GetRandomPosition();

            GameObject spawnedObject = Instantiate(prefabToSpawn2, (Vector2)transform.position + randomPosition, Quaternion.identity);
            spawnedObject.transform.parent = transform; // Установка текущего объекта в качестве родительского
        }
    }
    public void SpawnCheese()
    {
        for (int i = 0; i < numberOfPrefabs; i++)
        {
            Vector2 randomPosition = GetRandomPosition();

            GameObject spawnedObject = Instantiate(prefabToSpawn3, (Vector2)transform.position + randomPosition, Quaternion.identity);
            spawnedObject.transform.parent = transform; // Установка текущего объекта в качестве родительского
        }
    }

    public Vector2 GetRandomPosition()
    {
        float randomX = Random.Range(-spawnAreaSize.x / 2 * randomScale, spawnAreaSize.x / 2 * randomScale);
        float randomY = Random.Range(-spawnAreaSize.y / 2 * randomScale, spawnAreaSize.y / 2 * randomScale);

        // Дополнительный случайный сдвиг
        float randomOffsetX = Random.Range(-5f, 5f);
        float randomOffsetY = Random.Range(-5f, 5f);

        return new Vector2(randomX + randomOffsetX, randomY + randomOffsetY);
    }
}
