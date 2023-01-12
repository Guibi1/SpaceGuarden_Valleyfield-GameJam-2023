using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public CinemachineFreeLook cam;
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
        Cursor.lockState = CursorLockMode.Locked;
        instance = this;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        rb.velocity = (cameraVectorForward * targetPos.x + targetPos.z *  cameraVectorRight) * speedMultiplier;
        sprite.transform.LookAt(cam.transform);

        if (horizontalAxis > 0)
        {
            sprite.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (horizontalAxis < 0)
        {
            sprite.GetComponent<SpriteRenderer>().flipX = true;
        }


    }
    public void setRotation(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward;
        cameraVectorRight = right;
    }

}
