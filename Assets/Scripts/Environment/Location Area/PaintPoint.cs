using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintPoint : MonoBehaviour
{
    bool _distanceComlete = false;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && _distanceComlete)
        {
            _distanceComlete = false;
            IsPushed?.Invoke();
            gameObject.SetActive(false);
        }
    }

    public void DistanceComplete()
    {
        _distanceComlete = true;
    }

    internal event Action IsPushed;
}
