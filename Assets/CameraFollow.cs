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
    [SerializeField] private Vector3 cameraOffset;
    [SerializeField] private float cameraSmoothingSpeed;
    [SerializeField] private float cameraSmoothingRotationSpeed;
    public float sensitivity;


    [Header("Misc")]
    [SerializeField] private GameObject Camera_Rotate_Around;

    private void Start()
    {
        instance = this; // Un bon Singleton
        cam = Camera.main;
        cam.transform.localPosition = cameraOffset;
        targetRotation = Camera_Rotate_Around.transform.eulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        float camMovement = Input.GetAxis("Mouse X");
        targetRotation += new Vector3(0, camMovement * sensitivity, 0);
        Camera_Rotate_Around.transform.rotation = Quaternion.Lerp(Camera_Rotate_Around.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * cameraSmoothingRotationSpeed);
        Camera_Rotate_Around.transform.position = Vector3.Lerp(Camera_Rotate_Around.transform.position, transform.position, cameraSmoothingSpeed * Time.deltaTime);
        
    }
}
