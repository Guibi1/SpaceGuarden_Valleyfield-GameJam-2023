using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class BaseCampManager : MonoBehaviour
{
    public static BaseCampManager instance;

    [SerializeReference] Alien alien1Prefab;
    [SerializeReference] Alien alien2Prefab;
    [SerializeReference] Alien alien3Prefab;
    [SerializeReference] Transform bossSpawnLocation;
    [SerializeReference] List<Transform> spawnLocations;
    [SerializeReference] private TextMeshProUGUI overlayTextMesh;


    private bool isFighting = false;
    public int currentTurn = 0;
    public int turnsUntilNextShippement = 0;
    public Plant nextShippement;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        NextTurn();
    }

    void Update()
    {
        if (isFighting && AlienManager.instance.aliens.Count == 0)
        {
            isFighting = false;
            NextTurn();
        }

        if (turnsUntilNextShippement > 0)
        {
            overlayTextMesh.text = "Votre plante arrive dans " + turnsUntilNextShippement + " tour" + (turnsUntilNextShippement > 1 ? "s" : "");
        }
        else if (Vector3.Distance(transform.position, PlayerMouvement.instance.transform.position) <= PlayerMouvement.instance.distanceToInteract)
        {
            if (SpaceShip.instance.dummyPlantOnTop != null)
            {
                overlayTextMesh.text = "Appuyez sur E pour prendre la plante";
            }
            else
            {
                overlayTextMesh.text = "Appuyez sur E pour ouvrir la boutique";
            }
        }
        else
        {
            overlayTextMesh.text = "";
        }
    }

    [Button]
    private void NextTurn()
    {
        AlienManager.instance.killedAlien = 0;
        float cooldownNextWave = 5f;

        turnsUntilNextShippement -= 1;
        if (turnsUntilNextShippement == 0 && nextShippement != null)
        {
            SpaceShip.instance.GoDown(nextShippement);
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
            SpawnHorde(alien2Prefab, nbEnemies2 * (currentTurn/2));
            SpawnHorde(alien3Prefab, nbEnemies3);
        }));
    }


    public void SpawnHorde(Alien prefab, int count)
    {
        SpawnRecursive(prefab, 0, count);
    }

    private void SpawnRecursive(Alien prefab, int i, int max)
    {
        AlienManager.instance.spawnedAlien = i;
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
        if (turnsUntilNextShippement > 0) return;
        nextShippement = plant;
        turnsUntilNextShippement = plant.plantData.timeToShip;
        SpaceShip.instance.GoUp();
    }
}
