using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public static CameraMovement instance;
    Camera cam = null;

    private Vector3 targetRotation = Vector3.zero;

    [Header("Player variables")]
    [SerializeField] private float cameraOffset = 7;
    [SerializeField] private float cameraSmoothingRotationSpeed = 5;
    [SerializeField] private float cameraYMin = 0;
    [SerializeField] private float cameraYMax = 45;


    [Header("Settings")]
    public float sensitivity = 2;
    public float sensitivityYMultiplier = 0.05f;


    [Header("Misc")]
    [SerializeField] private GameObject Camera_Rotate_Around;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        cam = Camera.main;
        cam.transform.localPosition = new Vector3(0, 0, -cameraOffset);
        targetRotation = Camera_Rotate_Around.transform.eulerAngles;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        float camMovementX = Input.GetAxis("Mouse X");
        float camMovementY = Input.GetAxis("Mouse Y");

        float targetRotationX = camMovementY * sensitivity * sensitivityYMultiplier;
        if (targetRotation.x < cameraYMin)
            targetRotationX = cameraYMin;
        else if (targetRotation.x > cameraYMax)
            targetRotationX = cameraYMax;

        targetRotation += new Vector3(targetRotationX, camMovementX * sensitivity, 0);

        Camera_Rotate_Around.transform.rotation = Quaternion.Lerp(Camera_Rotate_Around.transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * cameraSmoothingRotationSpeed);
        Camera_Rotate_Around.transform.position = transform.position;
        Vector3 forward = Camera_Rotate_Around.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        forward.Normalize();
        PlayerMouvement.instance.SetCameraVectors(forward, Camera_Rotate_Around.transform.right);
    }
}
