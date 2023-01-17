using Lean.Pool;
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

    public void Plant(Plant prefab)
    {
        print(prefab.GetType());
        if(prefab.GetType() == typeof(Cherry))
        {
            ((Cherry)prefab).placed = true;
        }
        Plant plant = LeanPool.Spawn(prefab, transform).GetComponent<Plant>();
        plant.transform.localPosition = Vector3.zero - new Vector3(0,0.2f,0);
        PlantManager.instance.plants.Add(plant);
        PlayerMouvement.instance.notification.HideText();
    }

}

public enum TileState
{
    Disabled,
    Normal,
    Empty,
}
