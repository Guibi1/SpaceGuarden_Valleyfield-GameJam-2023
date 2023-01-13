using UnityEngine;

public class buttonFeedback : MonoBehaviour
{
    public float defaultSize;
    public float hoverSize;

    public void OnMouseEnter()
    {
        transform.localScale = new Vector3(hoverSize, hoverSize, hoverSize);
    }

    public void OnMouseExit()
    {
        transform.localScale = new Vector3(defaultSize, defaultSize, defaultSize);
    }
}
