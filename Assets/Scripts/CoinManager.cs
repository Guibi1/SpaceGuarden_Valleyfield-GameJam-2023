using Cinemachine;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using System.Diagnostics;

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
            // todo close the shop info in overlay canvas, in coinManager
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
        // todo open the shop info in overlay canvas, in coinManager
    }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CloseAll();
    }

    public TextMeshProUGUI moneyTMP;
    public TextMeshProUGUI roundTMP;

    void Update()
    {
        roundText.text = "Manche " + BaseCampManager.instance.currentTurn;
        roundTMP.text = "Manche " + BaseCampManager.instance.currentTurn;

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
    }

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
