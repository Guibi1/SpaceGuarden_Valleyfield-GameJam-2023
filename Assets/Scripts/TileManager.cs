using System.Collections;
using Shapes;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Color selectedColor;
    [SerializeField] Color emptyColor;

    [Header("Player variables")]
    public float animationSpeed = 16f;


    public Rectangle rectangle;

    private TileState _state = TileState.Empty;
    public TileState state
    {
        get => _state;
        set
        {
            switch (value)
            {
                case TileState.Empty:
                    rectangle.Color = Color.gray;
                    break;
            }

            _state = value;
        }
    }

    private GridManager parent;

    public bool selected { get; set; }

    void Start()
    {
        rectangle = GetComponentInChildren<Rectangle>();
        state = TileState.Empty;
    }

    void Update()
    {
        Color targetColor = selected ? selectedColor : emptyColor;
        float targetSize = selected ? .9f : .7f;

        rectangle.Color = Color.Lerp(rectangle.Color, targetColor, Time.deltaTime * animationSpeed);
        rectangle.Width = Mathf.Lerp(rectangle.Width, targetSize, Time.deltaTime * animationSpeed);
        rectangle.Height = Mathf.Lerp(rectangle.Height, targetSize, Time.deltaTime * animationSpeed);
    }

    public void Plant(Plant plant)
    {

        // TODO : Plant logic here

    }

}

public enum TileState
{
    Disabled,
    Normal,
    Empty,
}
