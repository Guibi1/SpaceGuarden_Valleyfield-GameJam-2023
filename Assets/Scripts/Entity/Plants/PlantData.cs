using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    [Header("Gameplay")]
    public int cost;
    public float health;
    public int timeToShip;

    [Header("Animation")]
    public float intervalBetweenExecute;
    public float executeTime;
    public float preparingTime;
    public float constructionTime;
    public float radius;
    public bool canPierce;
}
