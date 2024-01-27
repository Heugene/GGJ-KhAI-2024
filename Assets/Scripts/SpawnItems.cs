using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItems : MonoBehaviour
{
    public Transform[] itemSpawnPoints;
    public GameObject spawnItem;

    // Start is called before the first frame update
    void Start()
    {
        SpawnItemIfNotPresent();
    }

    // Update is called once per frame
    void Update()
    {
        // You can add some logic here if needed
    }

    void SpawnItemIfNotPresent()
    {
        foreach (Transform spawnPoint in itemSpawnPoints)
        {
            Collider[] colliders = Physics.OverlapSphere(spawnPoint.position, 0.2f); // Adjust the radius as needed

            // Check if there are no colliders (no objects) at the spawn point
            if (colliders.Length == 0)
            {
                Instantiate(spawnItem, spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}
