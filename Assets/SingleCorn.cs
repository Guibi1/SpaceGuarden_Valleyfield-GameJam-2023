using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCorn : MonoBehaviour
{

    public Transform target;
    public Rigidbody rb;
    
    public float speed = 1.0f;
    public float randomness = 0.1f;
    void Start()
    {
        StartCoroutine(SimpleRoutines.WaitTime(1, () =>
        {
            // Randomly adjust rotation speed and direction
            float randomSpeed = speed + Random.Range(-randomness, randomness);
            Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);

            // Rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, randomSpeed * Time.deltaTime);
            
            Vector3 targ = Vector3.MoveTowards(transform.position, target.position, Mathf.Infinity);
            targ.Normalize();
            GetComponent<Rigidbody>().velocity = targ * speed;
        }));

    }
}
