using Cinemachine;
using Lean.Pool;
using UnityEngine;

public class PlayerMouvement : MonoBehaviour
{
    public CinemachineFreeLook cam;
    public static PlayerMouvement instance;
    private Vector3 cameraVectorForward;
    private Vector3 cameraVectorRight;
    [SerializeField] public float xCamSpeed = 300f;

    [Header("Settings")]
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


    // Start is called before the first frame update
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

        // On space: toggle edit mode
        if (Input.GetKeyDown(KeyCode.Space) && playertype == PlayerTypes.Plant)
        {
            editMode = !editMode;
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
