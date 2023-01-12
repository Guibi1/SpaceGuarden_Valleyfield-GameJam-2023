using Lean.Pool;
using UnityEngine;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;

    private int turnsUntilNextShippement;
    private Plant nextShippement;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        if (turnsUntilNextShippement == 0 && nextShippement != null)
        {
            LeanPool.Spawn(nextShippement, transform);
            nextShippement = null;
        }
    }

    void NextTurn()
    {
        turnsUntilNextShippement -= 1;
        // TODO: Handle turns (spawn ennemies)
    }

    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
