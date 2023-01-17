using Lean.Pool;
using System;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [SerializeField] int tilesNumberX = 20;
    [SerializeField] int tilesNumberZ = 20;
    [SerializeField] float border = 1;
    [SerializeField] float tileSize = 1;
    [SerializeField] LayerMask mask;

    [SerializeReference] GameObject tilePrefab;

    [HideInInspector] public TileManager selectedTile;
    [HideInInspector] public bool editMode;

    private TileManager[,] tiles;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        CreateGrid();
    }

    void Update()
    {
        if (editMode)
        {
            RaycastMouse();
        }
        else if (selectedTile)
        {
            selectedTile.Selected = false;
            selectedTile = null;
        }
    }


    public void RaycastMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit, Mathf.Infinity, mask);

        if (hit.collider == null) return;

        if (!hit.collider.CompareTag("Tile")) return;
        TileManager tileManager = hit.collider.gameObject.GetComponent<TileManager>();

        if (selectedTile != null)
        {
            selectedTile.Selected = false;
        }

        selectedTile = tileManager;
        selectedTile.Selected = true;
    }


    private void CreateGrid()
    {
        tiles = new TileManager[tilesNumberX, tilesNumberZ];

        for (int x = 0; x < tilesNumberX; x++)
        {
            for (int z = 0; z < tilesNumberZ; z++)
            {
                GameObject tile = LeanPool.Spawn(tilePrefab, new Vector3(x * tileSize + transform.localPosition.x, transform.localPosition.y, z * tileSize + transform.localPosition.z), Quaternion.Euler(0, 0, 0), transform);
                tiles[x, z] = tile.GetComponent<TileManager>();
                tiles[x, z].maxSize = tileSize - border;
            }
        }
    }
}
