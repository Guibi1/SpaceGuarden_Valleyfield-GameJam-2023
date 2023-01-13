using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UI;

public class shopUiHover : MonoBehaviour
{
    private Sprite defaultState;
    public Sprite hoverState;
    public Image spriteRenderer;
    
    private void Start()
    {
        defaultState = spriteRenderer.sprite;
    }

    public void OnMouseEnter()
    {
        transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        spriteRenderer.sprite = hoverState;
    }

    public void OnMouseLeave()
    {
        spriteRenderer.sprite = defaultState;
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    
}