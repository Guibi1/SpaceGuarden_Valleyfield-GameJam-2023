using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using System.Diagnostics;
using UnityEngine.UI.ProceduralImage;
using FMODUnity;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [SerializeReference] private Canvas shopCanvas;
    [SerializeReference] private Canvas pauseCanvas;
    [SerializeReference] private Canvas deathCanvas;
    [SerializeReference] private Canvas overlayCanvas;
    [SerializeReference] private Canvas backgroundCanvas;
    [SerializeReference] private TextMeshProUGUI roundText;
    [SerializeReference] private TextMeshProUGUI moneyText;
    [SerializeReference] private TextMeshProUGUI overlayText;
    [SerializeReference] private TextMeshProUGUI ennemies;
    



    //MUSIC FIX I GUESS IDK
    public StudioEventEmitter music;
    public FMOD.Studio.EventInstance instanceMusic;
    public EventReference fmodEvent;
    private float enemies, healt;


    [Header("Camera")]
    [SerializeReference] private CinemachineFreeLook cam;
    [SerializeField] private float xCamSpeed = 300f;

    public Volume volume;

    private bool isPlaying = true;

    private int _coins = 20;
    public int coins
    {
        get => _coins;
        set
        {
            moneyText.text = value + " $";
            moneyTMP.text = value + " $";

            _coins = value;
        }
    }

   

    public void OpenShop()
    {
        if (isPlaying)
        {
            Time.timeScale = 0;
            shopCanvas.enabled = true;
            backgroundCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            cam.m_XAxis.m_MaxSpeed = 0;
            volume.sharedProfile.components[0].active = true;
            overlayText.enabled = false;
        }
    }

    public void OpenPause()
    {
        if (isPlaying)
        {
            CloseAll();
            isPlaying = false;

            Time.timeScale = 0;
            pauseCanvas.enabled = true;
            overlayCanvas.enabled = false;
            backgroundCanvas.enabled = true;
            Cursor.lockState = CursorLockMode.None;
            cam.m_XAxis.m_MaxSpeed = 0;
            volume.sharedProfile.components[0].active = true;
        }
    }

    public void OpenDeath()
    {
        instanceMusic.setParameterByName("GameState", 1);
        isPlaying = false;
        CloseAll();

        Time.timeScale = 0;
        deathCanvas.enabled = true;
        overlayCanvas.enabled = false;
        backgroundCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
        cam.m_XAxis.m_MaxSpeed = 0;
        volume.sharedProfile.components[0].active = true;
    }

    public void CloseAll()
    {
        isPlaying = true;
        Time.timeScale = 1;
        shopCanvas.enabled = false;
        pauseCanvas.enabled = false;
        deathCanvas.enabled = false;
        overlayCanvas.enabled = true;
        backgroundCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam.m_XAxis.m_MaxSpeed = xCamSpeed;
        volume.sharedProfile.components[0].active = false;
        overlayText.enabled = true;
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //music.Play();
        instanceMusic = RuntimeManager.CreateInstance(fmodEvent);
        instanceMusic.start();
        CloseAll();
        
    }

    private void OnDestroy()
    {
        music.Stop();
        instanceMusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    public TextMeshProUGUI moneyTMP;
    public TextMeshProUGUI roundTMP;


    public ProceduralImage ProceduralImage;
    public ProceduralImage flower;

    public Center center;
    void Update()
    {
        instanceMusic.setParameterByName("Enemies", AlienManager.instance.aliens.Count);
        instanceMusic.setParameterByName("Healt", 1);

        roundText.text = "Manche " + BaseCampManager.instance.currentTurn;
        roundTMP.text = "Manche " + BaseCampManager.instance.currentTurn;
        flower.fillAmount = (float) center._healthPoint /(float) center.maxHealth;
        
        if (AlienManager.instance.aliens.Count != 0)
        {
            ennemies.text = AlienManager.instance.killedAlien + "/" + AlienManager.instance.spawnedAlien;
            ProceduralImage.fillAmount = (float) AlienManager.instance.killedAlien / (float) AlienManager.instance.spawnedAlien;
        }
        else
        {
            ProceduralImage.fillAmount = 0;
            ennemies.text = "preparing";
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (overlayCanvas.enabled)
            {
                OpenPause();
            }
            else if (!deathCanvas.enabled)
            {
                CloseAll();
            }
        }

        pea.gameObject.SetActive(false);
        tomato.gameObject.SetActive(false);
        champi.gameObject.SetActive(false);
        cerise.gameObject.SetActive(false);
        
        if (_coins < 15)
        {
            pea.gameObject.SetActive(true);
        } 
        if (_coins < 30)
        {
            tomato.gameObject.SetActive(true);
        } 
        if(_coins < 45)
        {
            champi.gameObject.SetActive(true);
        } 
        if (_coins < 60)
        {
            cerise.gameObject.SetActive(true);
        }
    }

    public GameObject pea;
    public GameObject tomato;
    public GameObject champi;
    public GameObject cerise;

    public void Buy(Plant plant)
    {
        if (plant.plantData.cost > coins) return;

        this.coins -= plant.plantData.cost;
        CloseAll();
        BaseCampManager.instance.BuyPlant(plant);
    }

    public void GainCoins(int coins)
    {
        if (coins <= 0) return;
        this.coins += coins;
    }
}
