using Shapes;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] Color selectedColor;
    [SerializeField] Color emptyColor;
    [SerializeField] public float maxSize;

    [SerializeReference] public Rectangle rectangle;

    private TileState _state = TileState.Empty;
    public TileState State
    {
        get => _state;
        set
        {
            _state = value;
            UpdateRectangle();
        }
    }

    public float Size
    {
        get => rectangle.Width;
        private set
        {
            StartCoroutine(SimpleRoutines.LerpCoroutine(rectangle.Width, value, .2f, (size) =>
            {
                rectangle.Width = size;
                rectangle.Height = size;
            }));
        }
    }

    private Color _color;
    public Color Color
    {
        get => rectangle.Color;
        set
        {
            StartCoroutine(SimpleRoutines.LerpCoroutine(0, 1f, .2f,
                (t) => rectangle.Color = Color.Lerp(_color, value, t),
                () => _color = value
            ));
        }
    }

    private bool _selected = false;
    public bool Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            UpdateRectangle();
        }
    }

    void Start()
    {
        UpdateRectangle();
    }

    private void UpdateRectangle()
    {
        Color = Selected ? selectedColor : emptyColor;
        Size = maxSize * (Selected ? 1f : .8f);
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
