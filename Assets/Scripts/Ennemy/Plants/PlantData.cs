using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public abstract class PlantData : ScriptableObject
{
    public float intervalBetweenExecute;
    public float executeTime;
    public float health;
    public float constructionTime;
    public float radius;
    public bool canPierce;
}

