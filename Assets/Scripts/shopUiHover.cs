using UnityEngine;
using UnityEngine.UI;

public class shopUiHover : MonoBehaviour
{
    private Sprite defaultState;
    public Sprite hoverState;
    public Image spriteRenderer;
    public PlantData plantData;
    
    private void Start()
    {
        defaultState = spriteRenderer.sprite;
    }

    public void OnMouseEnter()
    {
        print(name);
        if (CoinManager.instance == null)
        {
            print("null");
        }
        
        if (plantData == null)
        {
            print("null2");
        }
        print("________");
        
        if (plantData.cost > CoinManager.instance.coins)
            return;
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        spriteRenderer.sprite = hoverState;
    }

    public void OnMouseLeave()
    {
        if (plantData.cost > CoinManager.instance.coins)
            return;
        
        spriteRenderer.sprite = defaultState;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
}