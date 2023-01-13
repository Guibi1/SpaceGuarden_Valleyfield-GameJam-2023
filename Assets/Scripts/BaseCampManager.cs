using Lean.Pool;
using UnityEngine;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;

    [SerializeReference] Alien alien1Prefab;
    [SerializeReference] Alien alien2Prefab;
    [SerializeReference] Alien alien3Prefab;

    private bool isFighting = true;
    private int currentTurn = 0;
    private int turnsUntilNextShippement = 0;
    private Plant nextShippement;

    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Update()
    {
        if (isFighting && AlienManager.instance.aliens.Count == 0)
        {
            isFighting = false;
            StartCoroutine(SimpleRoutines.WaitTime(5f, () => NextTurn()));
        }
    }

    void NextTurn()
    {
        currentTurn += 1;
        turnsUntilNextShippement -= 1;
        isFighting = true;

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
            Alien boss = LeanPool.Spawn(alien3Prefab, new Vector3(), Quaternion.identity);
            Vector3 scale = boss.gameObject.transform.localScale * 3;
            boss.gameObject.transform.localScale = scale;
            return;
        }

        int nbEnemies1 = Mathf.FloorToInt(4f * Mathf.Sqrt(2.5f * currentTurn) + 2f);
        int nbEnemies2 = currentTurn >= 5 ? Mathf.FloorToInt(1.25f * currentTurn) : 0;
        int nbEnemies3 = currentTurn >= 15 ? Mathf.FloorToInt((currentTurn / 5f) - 2f) : 0;

        SpawnHorde(alien1Prefab, nbEnemies1, new Vector3());
        SpawnHorde(alien2Prefab, nbEnemies2, new Vector3());
        SpawnHorde(alien3Prefab, nbEnemies3, new Vector3());
    }

    public void SpawnHorde(Alien prefab, int count, Vector3 position)
    {
        for (int i = 0; i < count; i++)
        {
            LeanPool.Spawn(prefab, position, Quaternion.identity);
        }
    }


    public void ShipPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
    }
}
