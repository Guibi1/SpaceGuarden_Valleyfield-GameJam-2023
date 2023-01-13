using Cinemachine;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [SerializeReference] private Canvas shopCanvas;
    [SerializeReference] private Canvas pauseCanvas;
    [SerializeReference] private Canvas overlayCanvas;
    [SerializeReference] private TextMeshProUGUI roundText;
    [SerializeReference] private TextMeshProUGUI moneyText;


    [Header("Camera")]
    [SerializeReference] private CinemachineFreeLook cam;
    [SerializeField] private float xCamSpeed = 300f;

    private int _coins = 100;
    public int coins
    {
        get => _coins;
        set
        {
            moneyText.text = value + " $";
            _coins = value;
        }
    }

    public void OpenShop()
    {
        shopCanvas.enabled = true;
        overlayCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        cam.m_XAxis.m_MaxSpeed = 0;
    }

    public void OpenPause()
    {
        pauseCanvas.enabled = true;
        overlayCanvas.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        cam.m_XAxis.m_MaxSpeed = 0;
    }

    public void CloseAll()
    {
        shopCanvas.enabled = false;
        pauseCanvas.enabled = false;
        overlayCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        cam.m_XAxis.m_MaxSpeed = xCamSpeed;
    }

    void Start()
    {
        instance = this;
        CloseAll();
    }

    void Update()
    {
        roundText.text = "Manche " + (BaseCampManager.instance.currentTurn + 1);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (overlayCanvas.enabled)
            {
                OpenPause();
            }
            else
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
