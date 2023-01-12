using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public Vector3 speed;
    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += speed * Time.deltaTime;
    }
}
