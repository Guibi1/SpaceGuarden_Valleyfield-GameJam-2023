using System;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;

    public List<Plant> plants = new List<Plant>();

    public Center center;

    private void OnEnable()
    {
        TileManager.OnPlantPlanted += NewPlant;
    }

    private void NewPlant(Plant obj)
    {
        plants.Add(obj);
    }

    private void Awake()
    {
        instance = this;
        
        Debug.unityLogger.logEnabled = false;
        
        #if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
        #endif
    }
}
