using Lean.Pool;
using UnityEngine;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;

    private int currentTurn = 0;
    private int turnsUntilNextShippement = 0;
    private Plant nextShippement;

    void Start()
    {
        instance = this;
    }

    void NextTurn()
    {
        currentTurn += 1;
        turnsUntilNextShippement -= 1;

        if (turnsUntilNextShippement == 0 && nextShippement != null)
        {
            LeanPool.Spawn(nextShippement, transform);
            nextShippement = null;
        }

        if (currentTurn == 10)
        {
            OptionsManager.instance.TrophyWon = true;
        }

        // TODO: Handle turns (spawn ennemies)
    }

    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
