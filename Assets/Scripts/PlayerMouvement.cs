using Cinemachine;
using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] private float distanceToInteract = 5f;
    [SerializeField] private float scytheDamage = 20;
    [SerializeField] private float scytheCooldown = 5;
    [SerializeField] private float plantHealSpeed = 30f;

    [Header("References")]
    [SerializeField] private GameObject sprite;
    [SerializeField] private GameObject notification;
    [SerializeField] private Plant plantPrefab;


    [SerializeField] public PlayerStates playerState = PlayerStates.Normal;

    private Plant plantToHeal;
    private Notification plantNotif;
    private float scytheLastUsed = 0f;

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

    public bool EditMode
    {
        get => SceneManager.GetActiveScene().name == "credits" ? false : GridManager.instance.editMode;
        set
        {
            GridManager.instance.editMode = value;

            notification.SetActive(!value);
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            cam.m_XAxis.m_MaxSpeed = value ? 0 : xCamSpeed;
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
        if (plantToHeal != null)
        {
            plantNotif.ShowText(plantToHeal.HP >= plantToHeal.plantData.health ? "Plant is full health" : "Hold E to heal plant");
        }
        scytheLastUsed += Time.deltaTime;

        // Look at cam
        sprite.transform.LookAt(cam.transform);

        // Move
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        float verticalAxis = Input.GetAxisRaw("Vertical");
        Vector3 targetPos = new Vector3(verticalAxis, 0, horizontalAxis).normalized;
        if (playerState == PlayerStates.Normal && !EditMode)
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
            if (!EditMode)
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
            if (Vector3.Distance(transform.localPosition, BaseCampManager.instance.transform.localPosition) <= distanceToInteract)
            {
                CoinManager.instance.shopIsOpen = true;
            }

            else if (playertype == PlayerTypes.Plant)
            {
                EditMode = !EditMode;
            }
        }
        else if (Input.GetKey(KeyCode.E))
        {
            if (plantToHeal != null)
            {
                plantToHeal.SetHealth(plantToHeal.HP + Time.deltaTime * plantHealSpeed);
            }
        }

        //TODO : REMOVE THIS SHIT
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
        if (playertype == PlayerTypes.Plant) return;
        if (scytheLastUsed < scytheCooldown) return;
        scytheLastUsed = 0;
        SpriteManager.instance.attack();

        List<Alien> delete = new List<Alien>();
        foreach (Alien a in AliensInRange)
        {
            if (a == null)
            {
                delete.Add(a);
                continue;
            }

            a.OnHit(scytheDamage);
        }

        foreach (Alien a in delete)
        {
            AliensInRange.Remove(a);
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlantHealZone"))
        {
            plantToHeal = other.gameObject.GetComponentInParent<Plant>();
            plantNotif = plantToHeal.GetComponentInChildren<Notification>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlantHealZone"))
        {
            other.gameObject.GetComponentInParent<Plant>().GetComponentInChildren<Notification>().HideText();
            plantToHeal = null;
            plantNotif = null;
        }
    }
}
