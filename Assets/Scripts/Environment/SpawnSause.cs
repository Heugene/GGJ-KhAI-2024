using System.Collections;
using UnityEngine;

public class SpawnSause : MonoBehaviour
{
    [SerializeField] GameObject spawnItem;
    [SerializeField] float cooldownTime;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private IEnumerator SpawnObject()
    {
        while(true)
        {
            if (transform.childCount < 1) {
                yield return new WaitForSeconds(cooldownTime);

                GameObject newObject = Instantiate(spawnItem, transform.position, Quaternion.identity);
                newObject.transform.parent = transform;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
