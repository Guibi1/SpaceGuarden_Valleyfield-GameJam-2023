using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusHover : MonoBehaviour
{
    public Vector3 startScale;

    public float sinPower;
    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = startScale + Vector3.one * ((Mathf.Sin(Time.time) * sinPower));
    }
}
