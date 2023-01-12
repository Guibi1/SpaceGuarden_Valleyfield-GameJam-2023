using UnityEngine;

[CreateAssetMenu]
public class PlantData : ScriptableObject
{
    [Header("Gameplay")]
    public int cost=10;
    public float health=20f;
    public float damage = 0f;
    public int timeToShip=1;

    [Header("Animation")]
    public float intervalBetweenExecute=1;
    public float executeTime=0.25f;
    public float preparingTime;
    public float constructionTime;
    public float radius;
    public bool canPierce;
}
