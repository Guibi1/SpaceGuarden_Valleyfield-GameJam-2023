using Lean.Pool;
using System;
using System.Security.Cryptography.X509Certificates;
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

        int nbEnemies1 = Mathf.FloorToInt(4f * Mathf.Sqrt(2.5f * currentTurn) + 10f);
        int nbEnemies2 = 0;
        int nbEnemies3 = 0;

        if (currentTurn >= 5)
        {
            nbEnemies2 = Mathf.FloorToInt(1.25f * currentTurn);
        }

        if (currentTurn >= 15)
        {
            nbEnemies3 = Mathf.FloorToInt((currentTurn / 5f) - 2f);
        }

        if (currentTurn == 10)
        {
            nbEnemies1 = 0;
            nbEnemies2 = 0;
            nbEnemies3 = 1;
        }
    }

    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
