using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashText : MonoBehaviour
{
    float x = 0;

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime * 2;
        float y = (Mathf.Sin(x) / 8 + 2) / 2;
        transform.localScale = new Vector3(y, y, y);
    }
}
