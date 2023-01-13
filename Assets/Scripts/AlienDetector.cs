using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDetector : MonoBehaviour
{
    public Transform target;

    private void OnTriggerStay(Collider other)
    {
        target = other.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
        }
    }
}
