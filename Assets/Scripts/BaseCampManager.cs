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

        int nbEnemies1 = Math.Floor(4*Math.Sqrt(2.5*currentTurn)+10)
        int nbEnemies2 = 0;
        int nbEnemies3 = 0;

        if (currentTurn >= 5)
        {
            enemies2 = Math.Floor(1.25 * currentTurn);
        }

        if (currentTurn >= 15)
        {
            enemies3 = Math.Floor((X500DistinguishedName/5)-2)
        }

        if (currentTurn == 10)
        {
            enemies1 = 0;
            enemies2 = 0;
            enemies13 = 1;
        }
    }

    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
