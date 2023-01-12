using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    [SerializeReference] GameObject tilePrefab;

    [SerializeReference] GameObject player;


    private GameObject[,] tiles;

    // Start is called before the first frame update
    void Start()
    {
        createGrid();
    }

    // Update is called once per frame
    void Update()
    {
        int x = Mathf.FloorToInt(player.transform.position.x - transform.position.x + 1f);
        int z = Mathf.FloorToInt(player.transform.position.z - transform.position.z + 1f);

        if (x >= 0 && z >= 0 && x < width && z < height)
        {
            Vector3 position = tiles[x, z].transform.position;
            position.y = 2;

            tiles[x, z].transform.position = position;
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
