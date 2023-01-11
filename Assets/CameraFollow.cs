using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Camera cam = null;
    Vector3 cameraOriginalPos;
    [SerializeField] private float cameraSpeed;
    private void Start()
    {
        cam = Camera.main;
        print("cam is " + cam.name);
        cameraOriginalPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position + cameraOriginalPos, cameraSpeed * Time.deltaTime);
    }
}
