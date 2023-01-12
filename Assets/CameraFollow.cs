using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public static CameraFollow instance;
    Camera cam = null;

    private Vector3 targetRotation = Vector3.zero;

    [Header("Player variables")]
    [SerializeField] private float cameraOffset = 7;
    [SerializeField] private float cameraSmoothingSpeed = 10;
    [SerializeField] private float cameraSmoothingRotationSpeed = 5;
    public float sensitivity = 2;
    public float sensitivityYMultiplier = 0.05f;


    [Header("Misc")]
    [SerializeField] private GameObject Camera_Rotate_Around;

    private void Start()
    {
        instance = this; // Un bon Singleton
        cam = Camera.main;
        cam.transform.localPosition = new Vector3(0, 0, -cameraOffset);
        targetRotation = Camera_Rotate_Around.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float camMovementX = Input.GetAxis("Mouse X");
        float camMovementY = Input.GetAxis("Mouse Y");

        targetRotation += new Vector3(camMovementY * sensitivity * sensitivityYMultiplier, camMovementX * sensitivity, 0);
        Camera_Rotate_Around.transform.rotation = Quaternion.Lerp(Camera_Rotate_Around.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * cameraSmoothingRotationSpeed);
        Camera_Rotate_Around.transform.position = Vector3.Lerp(Camera_Rotate_Around.transform.position, transform.position, cameraSmoothingSpeed * Time.deltaTime);
        
    }
}
