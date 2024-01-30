using System.Collections;
using UnityEngine;

public class SpawnSause : MonoBehaviour
{
    [SerializeField] Transform[] itemPoints;
    [SerializeField] GameObject spawnItem;
    [SerializeField] float cooldownTime;


    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnObject());
        StartCoroutine(SpawnAllSauces());
    }

    private IEnumerator SpawnAllSauces()
    {
        while (true)
        {
            foreach (var point in itemPoints)
            {
                //StartCoroutine(SpawnObject(point));
                if (transform.childCount < 1)
                {
                    yield return new WaitForSeconds(cooldownTime);

                    GameObject newObject = Instantiate(spawnItem, transform.position, Quaternion.identity);
                    newObject.transform.parent = point;
                }
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator SpawnObject(Transform spawnPoint)
    {
        if (transform.childCount < 1)
        {
            yield return new WaitForSeconds(cooldownTime);

            GameObject newObject = Instantiate(spawnItem, transform.position, Quaternion.identity);
            newObject.transform.parent = spawnPoint;
        }
    }
}
