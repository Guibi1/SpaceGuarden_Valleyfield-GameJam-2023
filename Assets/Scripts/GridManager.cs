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


    [SerializeReference] TileManager tilePrefab;
    [SerializeReference] GameObject player;
    [SerializeField] Camera cam;

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
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);

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
                tiles[x, z] = LeanPool.Spawn(tilePrefab, new Vector3(x * tileSize + transform.localPosition.x, 0, z * tileSize + transform.localPosition.z), Quaternion.Euler(0, 0, 0), transform);
                tiles[x, z].maxSize = tileSize - border;
            }
        }
    }
}
