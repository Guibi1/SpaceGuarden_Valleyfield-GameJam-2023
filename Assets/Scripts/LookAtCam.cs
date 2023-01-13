using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    [SerializeReference] List<GameObject> children = new List<GameObject>();

    void Update()
    {
        transform.eulerAngles = new Vector3(0, Camera.main.transform.eulerAngles.y, 0);
        children.ForEach((child) => child.transform.localEulerAngles = new Vector3(0, 0, 0));
    }
}
