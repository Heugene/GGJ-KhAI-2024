using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFireball : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnSpeed = 3.5f;
    [SerializeField] private float coneAngle = 15f;
    [SerializeField] private int numberOfFireballs = 5;  // Количество файерболов
    [SerializeField] private float timeBetweenSpawns = 0.3f;  // Время между спаунами

    private void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    // Корутина для спауна нескольких файерболов
    public IEnumerator SpawnFireballs()
    {
        for (int i = 0; i < numberOfFireballs; i++)
        {
            SpawnFireballAtPlayer();

            // Ждем определенное время перед следующим спауном
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void SpawnFireballAtPlayer()
    {
        // Создание фаербола
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // Настройка направления и скорости фаербола
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float randomConeAngle = Random.Range(-coneAngle / 2f, coneAngle / 2f);
        Vector3 finalDirection = Quaternion.Euler(0f, 0f, randomConeAngle) * directionToPlayer;

        // Назначение направления фаербола
        fireball.transform.right = finalDirection;

        // Установка угла поворота фаербола через transform.rotation.z
        float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Применение скорости фаербола
        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = finalDirection * spawnSpeed;
        }
        else
        {
            Debug.LogError("Rigidbody2D not found on the fireball prefab!");
        }
    }
}
