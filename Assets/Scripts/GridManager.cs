using Lean.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager instance;
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeReference] GameObject tilePrefab;

    [SerializeReference] GameObject player;
    [SerializeField] Camera cam;
    [HideInInspector] public GameObject selectedTile;
    private GameObject[,] tiles;
    public bool editMode;

    void Start()
    {
        instance = this;
        createGrid();
    }

    void Update()
    {
        if (editMode)
        {
            RaycastMouse();
        }
        else if (selectedTile != null) 
        {
            selectedTile.GetComponent<TileManager>().Selected = false;
        }
    }


    public void RaycastMouse()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (hit.collider == null) return;
        if (!hit.collider.CompareTag("Tile")) return;
        TileManager tileManager = hit.collider.gameObject.GetComponent<TileManager>();

        if (tileManager)
        {
            if (selectedTile != null)
            {
                selectedTile.GetComponent<TileManager>().Selected = false;
            }

            selectedTile = hit.collider.gameObject;
            selectedTile.GetComponent<TileManager>().Selected = true;
        }
    }


    private void createGrid()
    {
        tiles = new GameObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                tiles[x, z] = LeanPool.Spawn(tilePrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }
    }
}
