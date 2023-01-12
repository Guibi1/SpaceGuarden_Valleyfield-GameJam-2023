using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int height;

    public GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        createGrid();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void createGrid()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Instantiate(tilePrefab, new Vector3(i, j), Quaternion.identity, transform);
            }
        }
    }
}
