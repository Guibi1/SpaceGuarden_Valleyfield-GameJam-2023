using Cinemachine;
using DG.Tweening;
using Lean.Pool;
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
    private float xSpeedSave; 

    [Header ("Settings")]
    public float speedMultiplier = 50f;
    public float spinTime = 8f;
    public float knockbackForce = 2000f;
    public int knockbackFrames = 4;
    [HideInInspector] public Rigidbody rb;

    [Header("Misc")]
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject notification;
    [SerializeField] private Plant plantPrefab;

    public PlayerStates playerState;
    public PlayerTypes playertype;
    

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        instance = this;
        rb = GetComponent<Rigidbody>();
        xSpeedSave = cam.m_XAxis.m_MaxSpeed;
    }
    void Update()
    {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        if (playerState == PlayerStates.Normal && !GridManager.instance.editMode)
        {
            rb.velocity = (cameraVectorForward * targetPos.x + targetPos.z * cameraVectorRight) * speedMultiplier;
            if (horizontalAxis > 0)
            {
                sprite.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (horizontalAxis < 0)
            {
                sprite.transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        sprite.transform.LookAt(cam.transform);

        

        SpriteManager.instance.SetWalking(!(horizontalAxis == 0 && verticalAxis == 0));


        if (Input.GetKeyDown(KeyCode.Mouse0) && playerState == PlayerStates.Normal)
        {
            if (!GridManager.instance.editMode)
            {
                Fire();
            }
            else 
            {
                if(GridManager.instance.selectedTile != null)
                {
                    // TODO : Change this to actual plant to plant
                    Plant plant = LeanPool.Spawn(plantPrefab).GetComponent<Plant>();
                    GridManager.instance.selectedTile.GetComponent<TileManager>().Plant(plant);
                }
            }
        }
        

        if (playertype == PlayerTypes.Plant_Bald || playertype == PlayerTypes.Plant_Hat)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GridManager.instance.editMode = !GridManager.instance.editMode;
                if (GridManager.instance.editMode)
                {
                    notification.SetActive(false);
                    Cursor.lockState = CursorLockMode.None;
                    cam.m_XAxis.m_MaxSpeed = 0;
                }
                else
                {
                    notification.SetActive(true);
                    print(xSpeedSave);
                    cam.m_XAxis.m_MaxSpeed = xSpeedSave;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        // TODO : REMOVE THIS SHIT
        if (Input.GetKeyDown(KeyCode.P))
        {
            notification.SetActive(true);
            playertype = PlayerTypes.Plant_Hat;
            SpriteManager.instance.SetCharType(PlayerTypes.Plant_Bald);

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
        // TODO : Activate animation
        rb.velocity = (sprite.transform.localScale.x == 1 ?
            cameraVectorRight * -knockbackForce : // Facing left
            cameraVectorRight * knockbackForce  // Facing right
            );
        StartCoroutine(SimpleRoutines.WaitForFrames(knockbackFrames, () =>
        {
            playerState = PlayerStates.Normal;
        }));
    }


}
