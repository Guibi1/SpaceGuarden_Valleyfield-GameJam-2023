using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Textscroll : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 1 * Time.deltaTime, 0);        
    }
}
