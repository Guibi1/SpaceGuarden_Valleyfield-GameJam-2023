using System.Collections;
using Shapes;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Color selectedColor;
    [SerializeField] Color emptyColor;

    private Rectangle rectangle;

    private TileState _state = TileState.Empty;
    public TileState State
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

    public bool Selected { get; set; }

    void Start()
    {
        rectangle = GetComponentInChildren<Rectangle>();
        State = TileState.Empty;
    }

    void Update()
    {
        Color targetColor = Selected ? selectedColor : emptyColor;
        float targetSize = Selected ? .9f : .7f;


        rectangle.Color = Color.Lerp(rectangle.Color, targetColor, Time.deltaTime * 8f);
        rectangle.Width = Mathf.Lerp(rectangle.Width, targetSize, Time.deltaTime * 8f);
        rectangle.Height = Mathf.Lerp(rectangle.Height, targetSize, Time.deltaTime * 8f);
    }
}

public enum TileState
{
    Disabled,
    Normal,
    Empty,
}
