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
    [SerializeField] public float distanceToInteract = 5f;
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

    [SerializeField] private PlayerTypes _playerType = PlayerTypes.Scythe;
    public PlayerTypes PlayerType
    {
        get => _playerType;
        set
        {
            _playerType = value;
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
            plantNotif.ShowText(plantToHeal.HP >= plantToHeal.plantData.health ? "La plante est en bonne santé" : "Laissez Espace enfoncé pour soigner la plante");
        }

        if (EditMode)
        {
            notification.ShowText("Cliquer sur une case pour la planter");
        }
        else if (plantPrefab != null)
        {
            notification.ShowText("Appuyer sur Q pour choisir où planter votre semence");
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
                Attack();
            }
            else if (GridManager.instance.selectedTile != null)
            {
                GridManager.instance.selectedTile.Plant(plantPrefab);
                EditMode = false;
                plantPrefab = null;
                PlayerType = PlayerTypes.Scythe;
            }
        }

        // On interact
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (plantToHeal == null)
            {
                // Shop
                if (Vector3.Distance(transform.position, BaseCampManager.instance.transform.position) <= distanceToInteract)
                {
                    if (SpaceShip.instance.dummyPlantOnTop != null)
                    {
                        SpaceShip.instance.PickUp();
                    }
                    else if (BaseCampManager.instance.turnsUntilNextShippement <= 0)
                    {
                        CoinManager.instance.OpenShop();
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            // Edit mode
            if (PlayerType == PlayerTypes.Plant)
            {
                EditMode = !EditMode;
            }
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            // Heal
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
        plantPrefab = plant;
        PlayerType = PlayerTypes.Plant;
    }


    public void SetCameraVectors(Vector3 forward, Vector3 right)
    {
        cameraVectorForward = forward.normalized;
        cameraVectorRight = right.normalized;
    }

    public void Attack()
    {
        if (PlayerType == PlayerTypes.Plant) return;
        if (scytheLastUsed < scytheCooldown) return;

        scytheLastUsed = 0;
        SpriteManager.instance.attack();
        emitterSwing.Play();

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
            plantNotif = plantToHeal.notification;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlantHealZone"))
        {
            if (plantNotif)
            {
                plantNotif.HideText();
            }

            if (plantToHeal) {
                plantToHeal.plantSweat.Stop();
                plantToHeal = null;
                plantNotif = null;
            }

        }
    }
}
