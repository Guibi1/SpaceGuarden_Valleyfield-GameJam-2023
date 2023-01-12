using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager instance;

    [SerializeReference] Toggle baldToggle;

    void Awake()
    {
        instance = this;

        if (baldToggle != null)
        {
            baldToggle.isOn = bald;
        }
    }

    public bool bald
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

    public void UpdateBaldFromToggle()
    {
        bald = baldToggle.isOn;
    }
}
