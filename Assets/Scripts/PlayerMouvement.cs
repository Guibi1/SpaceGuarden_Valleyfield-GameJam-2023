using Cinemachine;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public static PlayerMouvement instance;

    [HideInInspector] public Rigidbody rb;

    [Header("Camera")]
    [SerializeReference] public CinemachineFreeLook cam;
    private Vector3 cameraVectorForward;
    private Vector3 cameraVectorRight;
    [SerializeField] public float xCamSpeed = 300f;

    [Header("Settings")]
    [SerializeField] public float speedMultiplier = 50f;
    [SerializeField] public float distanceToInteract = .5f;

    [Header("References")]
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject baseCamp;
    [SerializeField] private Plant plantPrefab;

    public PlayerStates playerState;

    private PlayerTypes _playertype;
    public PlayerTypes playertype
    {
        get => _playertype;
        set
        {
            _playertype = value;
            SpriteManager.instance.SetCharType(value);
        }
    }

    public bool editMode
    {
        get => GridManager.instance.editMode;
        set
        {
            GridManager.instance.editMode = value;

            if (value)
            {
                notification.SetActive(false);
                Cursor.lockState = CursorLockMode.None;
                cam.m_XAxis.m_MaxSpeed = 0;
            }
            else
            {
                notification.SetActive(true);
                Cursor.lockState = CursorLockMode.Locked;
                cam.m_XAxis.m_MaxSpeed = xCamSpeed;
            }
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        instance = this;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Look at cam
        sprite.transform.LookAt(cam.transform);

        // Move
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        if (playerState == PlayerStates.Normal && !editMode)
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
        SpriteManager.instance.SetWalking(!(horizontalAxis == 0 && verticalAxis == 0));

        // On mouse click
        if (Input.GetKeyDown(KeyCode.Mouse0) && playerState == PlayerStates.Normal)
        {
            if (!editMode)
            {
                Fire();
            }
            else if (GridManager.instance.selectedTile != null)
            {
                Plant plant = LeanPool.Spawn(plantPrefab).GetComponent<Plant>();
                GridManager.instance.selectedTile.Plant(plant);
            }
        }

        // On interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(transform.position, transform.position) <= distanceToInteract)
            {
                CoinManager.instance.shopIsOpen = true;
            }
            else if (playertype == PlayerTypes.Plant)
            {
                editMode = !editMode;
            }
        }

        //! TODO : REMOVE THIS SHIT
        if (Input.GetKeyDown(KeyCode.P))
        {
            notification.SetActive(true);
            playertype = PlayerTypes.Plant;
        }
    }

    public void SetCameraVectors(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward;
        cameraVectorRight = right;
    }

    public void Fire()
    {
        SpriteManager.instance.attack();
    }

    public List<Alien> AliensInRange = new List<Alien>();
    public void addAlien(Alien a)
    {
        AliensInRange.Add(a);
    }

    public void removeAlien(Alien a)
    {
        AliensInRange.Remove(a);
    }

}
