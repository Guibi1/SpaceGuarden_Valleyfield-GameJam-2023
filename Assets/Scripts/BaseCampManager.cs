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

        if (currentTurn == 11)
        {
            OptionsManager.instance.TrophyWon = true;
        }

        if (currentTurn == 10)
        {
            //spawn big #3 (scale * 3?)
            return;
        }

        int nbEnemies1 = Mathf.FloorToInt(4f * Mathf.Sqrt(2.5f * currentTurn) + 10f);
        int nbEnemies2 = currentTurn >= 5 ? Mathf.FloorToInt(1.25f * currentTurn) : 0;
        int nbEnemies3 = currentTurn >= 15 ? Mathf.FloorToInt((currentTurn / 5f) - 2f) : 0;

        // TODO: Handle turns (spawn ennemies)
    }

    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
