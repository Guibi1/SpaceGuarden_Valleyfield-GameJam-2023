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
    [SerializeField] int width;
    [SerializeField] int height;
    [SerializeField] float border = 1;
    [SerializeField] float size = 1;
    

    [SerializeReference] TileManager tilePrefab;

    [SerializeReference] GameObject player;
    [SerializeField] Camera cam;

    [HideInInspector] public TileManager selectedTile;

    private TileManager[,] tiles;
    public bool editMode;

    private void Awake()
    {
        instance = this;

    }

    void Start()
    {

        tilePrefab.rectangle.Width = size;
        tilePrefab.rectangle.Height = size;
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
            selectedTile.selected = false;
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
            selectedTile.selected = false;
        }

        selectedTile = tileManager;
        selectedTile.selected = true;
    }


    private void CreateGrid()
    {
        tiles = new TileManager[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                tiles[x, z] = LeanPool.Spawn(tilePrefab, new Vector3((x * size) + transform.localPosition.x, 0, (z * size) + transform.localPosition.z), Quaternion.Euler(0, 0, 0), transform);
                tiles[x, z].rectangle.Height = size;
                tiles[x, z].rectangle.Width = size;
            }
        }
    }
}
