using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeReference] GameObject tilePrefab;

    [SerializeReference] GameObject player;


    private GameObject[,] tiles;

    private TileManager selectedTile;

    void Start()
    {
        createGrid();
    }

    void Update()
    {
        int x = Mathf.FloorToInt(player.transform.position.x - transform.position.x + 1f);
        int z = Mathf.FloorToInt(player.transform.position.z - transform.position.z + 1f);

        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            TileManager newSelection = tiles[x, z].GetComponent<TileManager>();

            if (selectedTile != newSelection)
            {
                if (selectedTile != null)
                {
                    selectedTile.Selected = false;
                }

                newSelection.Selected = true;
                selectedTile = newSelection;
            }
        }

        else
        {
            if (selectedTile != null)
            {
                selectedTile.Selected = false;
                selectedTile = null;
            }
        }
    }

    private void createGrid()
    {
        tiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                tiles[x, z] = Instantiate(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
    }
}
