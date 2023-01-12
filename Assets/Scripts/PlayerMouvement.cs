using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerMouvement : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public static PlayerMouvement instance;
    private Vector3 cameraVectorForward;
    private Vector3 cameraVectorRight;

    [Header ("Settings")]
    public float speedMultiplier = 50f;
    public float spinTime = 8f;
    public float knockbackForce = 10000f;

    [HideInInspector] public Rigidbody rb;

    [Header("Misc")]
    [SerializeField] private GameObject sprite;
    public PlayerStates playerState;
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
        if (playerState == PlayerStates.Normal)
        {
            rb.velocity = (cameraVectorForward * targetPos.x + targetPos.z * cameraVectorRight) * speedMultiplier;
        }
        sprite.transform.LookAt(cam.transform);

        if (horizontalAxis > 0)
        {
            sprite.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalAxis < 0)
        {
            sprite.transform.localScale = new Vector3(-1, 1, 1);

        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && playerState == PlayerStates.Normal)
        {
            Fire();
        }
       
    }
    public void setRotation(Vector3 forward, Vector3 right)
    {
        
        cameraVectorForward = forward;
        cameraVectorRight = right;
    }

    public void Fire()
    {
        playerState = PlayerStates.Knockback;
        print("Hello, world!");
        // TODO : Activate animation
        rb.AddForce(sprite.transform.localScale.x == 1 ?
            cameraVectorRight * -knockbackForce : // Facing left
            cameraVectorRight * knockbackForce  // Facing right
            );
        StartCoroutine(SimpleRoutines.WaitTime(1, () =>
        {
            playerState = PlayerStates.Normal;
        }));
    }


}
