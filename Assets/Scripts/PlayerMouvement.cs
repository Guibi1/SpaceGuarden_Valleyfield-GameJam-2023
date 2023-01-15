using Cinemachine;
using Lean.Pool;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.SceneManagement;
using System;

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
    [SerializeField] private float scytheDamage = 2;
    [SerializeField] private float scytheCooldown = 5;
    [SerializeField] private float plantHealSpeed = 30f;

    [Header("References")]
    [SerializeField] private GameObject sprite;
    public Notification notification;
    [SerializeField] private Plant plantPrefab;


    [SerializeField] public PlayerStates playerState = PlayerStates.Normal;

    private Plant plantToHeal;
    private Notification plantNotif;
    private float scytheLastUsed = 0f;

    private GameObject instruction;

    public StudioEventEmitter emitterWalk;
    public StudioEventEmitter emitterSwing;
    public StudioEventEmitter emitterHeal;

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

            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            cam.m_XAxis.m_MaxSpeed = value ? 0 : xCamSpeed;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (plantToHeal != null)
        {
            plantNotif.ShowText(plantToHeal.HP >= plantToHeal.plantData.health ? "La plante est en bonne santé" : "Laissez E enfoncé pour soigner la plante");
        }

        if (EditMode)
        {
            notification.ShowText("Cliquer sur une case loin de vous pour la planter");
        }
        else if (plantPrefab != null)
        {
            notification.ShowText("Appuyer sur E pour planter votre semence");
        }
        else
        {
            if (notification != null)
            {
                notification.HideText();
            }
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
                emitterSwing.Play();
            }
            else if (GridManager.instance.selectedTile != null)
            {
                GridManager.instance.selectedTile.Plant(plantPrefab);
                EditMode = false;
                plantPrefab = null;
                playertype = PlayerTypes.Scythe;
            }
        }

        // On interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Shop
            if (Vector3.Distance(transform.position, BaseCampManager.instance.transform.position) <= distanceToInteract)
            {
                if (SpaceShip.instance.referenceVaisseau != null)
                {
                    SpaceShip.instance.PickUp();
                }
                else if (BaseCampManager.instance.turnsUntilNextShippement <= 0)
                {
                    CoinManager.instance.OpenShop();
                }
            }
            // Edit mode
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
                if (!plantToHeal.plantSweat.isPlaying) plantToHeal.plantSweat.Play();
            }
        }
        else
        {
            if (plantToHeal != null)
            {
                if (plantToHeal.plantSweat.isPlaying)
                {
                    plantToHeal.plantSweat.Stop();
                }
            }
        }
    }

    public void PickUpPlant(Plant plant)
    {
        notification.ShowText("Choisissez un emplacement où placer la plante");
        plantPrefab = plant;
        playertype = PlayerTypes.Plant;
    }


    public void SetCameraVectors(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward.normalized;
        cameraVectorRight = right.normalized;
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
            try
            {
                other.gameObject.GetComponentInParent<Plant>().GetComponentInChildren<Notification>().HideText();
                plantToHeal.plantSweat.Stop();
            }
            catch (Exception) { }
            plantToHeal = null;
            plantNotif = null;
        }
    }
}
