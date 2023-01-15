using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    public List<Plant> plants = new List<Plant>();

    public Center center;

    private void Awake()
    {
        instance = this;
    }
}
