using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPoint : MonoBehaviour
{
    bool _distanceComlete;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _distanceComlete)
        {
            GetComponentInParent<Transform>().parent.parent.GetComponent<PentagramLogic>().NextPath();
        }
    }

    public void DistanceComplete()
    {
        _distanceComlete = true;
    }
}
