using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;

    [SerializeReference] Toggle baldToggle;
    [SerializeReference] Image trophyImage;
    [SerializeReference] Sprite trophyWonSprite;

    void Awake()
    {
        instance = this;

        if (baldToggle != null)
        {
            baldToggle.isOn = Bald;
        }
    }

    public bool Bald
    {
        get => PlayerPrefs.GetInt("bald", 0) == 1;
        set
        {
            PlayerPrefs.SetInt("bald", value ? 1 : 0);

            if (SpriteManager.instance != null)
            {
                SpriteManager.instance.SetBald(value);
            }
        }
    }

    public bool TrophyWon
    {
        get => PlayerPrefs.GetInt("trophy", 0) == 1;
        set
        {
            PlayerPrefs.SetInt("trophy", value ? 1 : 0);

            if (trophyImage != null && value)
            {
                trophyImage.sprite = trophyWonSprite;
            }
        }
    }

    public void UpdateBaldFromToggle()
    {
        Bald = baldToggle.isOn;
    }
}
