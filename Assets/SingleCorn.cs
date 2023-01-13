using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCorn : MonoBehaviour
{

    public float speed;
    public Transform target;
    void Start()
    {
        StartCoroutine(SimpleRoutines.WaitTime(1, () =>
        {
            Vector3 targ = Vector3.MoveTowards(transform.position, target.position, Mathf.Infinity);
            targ.Normalize();
            GetComponent<Rigidbody>().velocity = targ * speed;
        }));

    }
}
