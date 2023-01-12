using Cinemachine;
using Lean.Pool;
using System;
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
    [SerializeField] private float speedMultiplier = 50f;
    [SerializeField] private float distanceToInteract = .5f;
    [SerializeField] private float scytheDamage = 20;
    [SerializeField] private float scytheCooldown = 5;
    [SerializeField] private float plantHealSpeed = 30f;

    [Header("References")]
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject notification;
    [SerializeField] private GameObject baseCamp;
    [SerializeField] private Plant plantPrefab;
    
    private Plant plantToHeal;

    private float scytheLastUsed = 0f;

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
        if(plantToHeal!= null) {
            if (plantToHeal.HP >= plantToHeal.plantData.health)
            {
                OnNotif?.Invoke("Plant is full health");
            }
            else
            {
                OnNotif?.Invoke("Hold E to heal plant");
            }
        }




        scytheLastUsed += Time.deltaTime;
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
<<<<<<< Updated upstream
            if (Vector3.Distance(transform.position, BaseCampManager.instance.transform.position) <= distanceToInteract)
=======
            // TODO : Fix this awful distance query please thank you
            if (Vector3.Distance(transform.position, transform.position) <= distanceToInteract && false)
>>>>>>> Stashed changes
            {
                CoinManager.instance.shopIsOpen = true;
            }
            else if (playertype == PlayerTypes.Plant)
            {
                editMode = !editMode;
            }
        }
<<<<<<< Updated upstream
=======
        else if (Input.GetKey(KeyCode.E))
        {
            if (plantToHeal != null)
            {
                print("Ohhh mon dieuu");
                plantToHeal.SetHealth(plantToHeal.HP + Time.deltaTime * plantHealSpeed);
            }
        }

        //TODO : REMOVE THIS SHIT
        if (Input.GetKeyDown(KeyCode.P))
        {
            notification.SetActive(true);
            playertype = PlayerTypes.Plant;
        }
>>>>>>> Stashed changes
    }



    public void SetCameraVectors(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward;
        cameraVectorRight = right;
    }

    public void Fire()
    {
        if (playertype == PlayerTypes.Plant) return;
        if (scytheLastUsed < scytheCooldown) return;
        scytheLastUsed = 0;
        SpriteManager.instance.attack();
        foreach(Alien a in AliensInRange)
        {
            a.OnHit(scytheDamage);
        }
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

    public static event Action<String> OnNotif;
    public static event Action KillNotif;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlantHealZone"))
        {
            plantToHeal = other.gameObject.GetComponentInParent<Plant>();
            print("selected plant : " + plantToHeal);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlantHealZone"))
        {
            KillNotif?.Invoke();
            plantToHeal = null;
        }
    }


}
