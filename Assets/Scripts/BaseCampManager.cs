using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using TMPro;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;

    [SerializeReference] Alien alien1Prefab;
    [SerializeReference] Alien alien2Prefab;
    [SerializeReference] Alien alien3Prefab;
    [SerializeReference] Transform bossSpawnLocation;
    [SerializeReference] List<Transform> spawnLocations;
    [SerializeField] private TextMeshPro textMesh;


    private bool isFighting = false;
    public int currentTurn = 0;
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
        NextTurn();
    }

    void Update()
    {
        if (isFighting && AlienManager.instance.aliens.Count == 0)
        {
            isFighting = false;
            NextTurn();
        }


        if (nextShippement != null)
        {
            textMesh.text = "Votre plante arrive dans " + turnsUntilNextShippement + " tours";
        }
        else
        {
            textMesh.text = "Appuyez sur E pour ouvrir la boutique";
        }
    }

    private void NextTurn()
    {
        float cooldownNextWave = 5f;

        turnsUntilNextShippement -= 1;
        if (turnsUntilNextShippement == 0 && nextShippement != null)
        {
            SpaceShip.instance.GoDown(nextShippement);
            nextShippement = null;
            cooldownNextWave = 10f;
        }

        if (currentTurn == 10)
        {
            OptionsManager.instance.TrophyWon = true;
        }

        float waitTime = cooldownNextWave;

        if (currentTurn == 0)
            waitTime = 10f;

        StartCoroutine(SimpleRoutines.WaitTime(waitTime, () =>
        {
            currentTurn += 1;
            isFighting = true;

            if (currentTurn == 10)
            {
                Alien boss = LeanPool.Spawn(alien3Prefab, bossSpawnLocation.position, Quaternion.identity);

                Vector3 localScale = boss.gameObject.transform.localScale;
                StartCoroutine(SimpleRoutines.LerpCoroutine(1f, 3f, 3f, (x) =>
                {
                    boss.gameObject.transform.localScale = localScale * x;
                }));
                boss.alienData.health = 200;
                boss.alienData.damage = 10;
                return;
            }

            int nbEnemies1 = Mathf.FloorToInt(2f * Mathf.Sqrt(2.5f * currentTurn) + 2f);
            int nbEnemies2 = currentTurn >= 5 ? Mathf.FloorToInt(1.25f * currentTurn) : 0;
            int nbEnemies3 = currentTurn >= 15 ? Mathf.FloorToInt((currentTurn / 5f) - 2f) : 0;

            SpawnHorde(alien1Prefab, nbEnemies1);
            SpawnHorde(alien2Prefab, nbEnemies2);
            SpawnHorde(alien3Prefab, nbEnemies3);
        }));
    }


    public void SpawnHorde(Alien prefab, int count)
    {
        SpawnRecursive(prefab, 0, count);
    }

    private void SpawnRecursive(Alien prefab, int i, int max)
    {
        if (i == max)
            return;

        LeanPool.Spawn(prefab, spawnLocations[i % spawnLocations.Count].position, Quaternion.identity);
        StartCoroutine(SimpleRoutines.WaitTime(1f / spawnLocations.Count, () => SpawnRecursive(prefab, i + 1, max)));
    }

    public void SpawnHorde(Alien prefab, int count, Vector3 position)
    {
        for (int i = 0; i < count; i++)
        {
            LeanPool.Spawn(prefab, position, Quaternion.identity);
        }
    }


    public void BuyPlant(Plant plant)
    {
        if (turnsUntilNextShippement != 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
        SpaceShip.instance.GoUp();
    }
}
