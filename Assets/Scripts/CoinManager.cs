using Cinemachine;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    [SerializeReference] public Canvas shopCanvas;

    [Header("Camera")]
    [SerializeReference] public CinemachineFreeLook cam;
    [SerializeField] public float xCamSpeed = 300f;

    public int coins { get; private set; } = 100;
    public bool shopIsOpen
    {
        get => shopCanvas.enabled;
        set
        {
            shopCanvas.enabled = value;
            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            cam.m_XAxis.m_MaxSpeed = value ? 0 : xCamSpeed;
        }
    }

    void Start()
    {
        instance = this;
        shopCanvas.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopIsOpen = false;
        }
    }

    public void Buy(Plant plant)
    {
        if (plant.plantData.cost > coins) return;

        this.coins -= plant.plantData.cost;
        shopIsOpen = false;
        BaseCampManager.instance.ShipPlant(plant);
    }

    public void GainCoins(int coins)
    {
        if (coins <= 0) return;
        this.coins += coins;
    }
}
