using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public static PlayerMouvement instance;
    private Vector3 cameraVectorForward;
    private Vector3 cameraVectorRight;
    

    [Header ("Settings")]
    public float speedMultiplier = 50;

    [HideInInspector] public Rigidbody rb;

    [Header("Misc")]
    [SerializeField] private GameObject sprite;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        print("target pos : " + targetPos);
        print("camera pos : " + cameraVectorForward);
        rb.velocity = (cameraVectorForward * targetPos.x + targetPos.z *  cameraVectorRight) * speedMultiplier;
        sprite.transform.LookAt(Camera.main.transform.position);
    }
    public void setRotation(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward;
        cameraVectorRight = right;
    }

}
