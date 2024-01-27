using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnFireball : MonoBehaviour
{
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform player;
    [SerializeField] private float spawnSpeed = 3.5f;
    [SerializeField] private float coneAngle = 15f;
    [SerializeField] private int numberOfFireballs = 5;  // ���������� ����������
    [SerializeField] private float timeBetweenSpawns = 0.3f;  // ����� ����� ��������

    private void Start()
    {
        if (player == null)
            player = GameObject.FindWithTag("Player").transform;
    }

    // �������� ��� ������ ���������� ����������
    public IEnumerator SpawnFireballs()
    {
        for (int i = 0; i < numberOfFireballs; i++)
        {
            SpawnFireballAtPlayer();

            // ���� ������������ ����� ����� ��������� �������
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    public void SpawnFireballAtPlayer()
    {
        // �������� ��������
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        // ��������� ����������� � �������� ��������
        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float randomConeAngle = Random.Range(-coneAngle / 2f, coneAngle / 2f);
        Vector3 finalDirection = Quaternion.Euler(0f, 0f, randomConeAngle) * directionToPlayer;

        // ���������� ����������� ��������
        fireball.transform.right = finalDirection;

        // ��������� ���� �������� �������� ����� transform.rotation.z
        float angle = Mathf.Atan2(finalDirection.y, finalDirection.x) * Mathf.Rad2Deg;
        fireball.transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // ���������� �������� ��������
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
