using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using FMODUnity;


public class Alien : Agent
{
    public enum BehaviorState { Inactive, WantsPlant, WantsCenter }

    [SerializeField] private Animator alienAnimator;

    [ReadOnly]
    public BehaviorState Behavior;

    public enum SearchState { Inactive, Center, Plant, AttackCenter, AttackPlant }

    [ReadOnly]
    public SearchState searchState;

    private Plant currentTargetPlant;

    [InlineEditor()]
    public AlienData alienData;

    public BehaviorState StartingBehaviorState;

    [Button]
    public void StartAlienBrain()
    {
        Behavior = StartingBehaviorState;
    }

    [SerializeField] private GameObject sprite;

    public StudioEventEmitter emmitterKill;
    public StudioEventEmitter emmiterHit;

    private void OnEnable()
    {
        StartAlienBrain();
    }


    public override void Start()
    {
        healthBar.maxHealth = alienData.health;
        HP = alienData.health;

        if (!AlienManager.instance.aliens.Contains(this))
            AlienManager.instance.aliens.Add(this);
    }

    private void Update()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.Lerp(sprite.GetComponent<SpriteRenderer>().color, Color.white, Time.deltaTime * 8f);


        switch (Behavior)
        {
            case BehaviorState.Inactive:
                return;
            case BehaviorState.WantsPlant:

                SearchSwitchCase(SearchState.Plant);

                break;
            case BehaviorState.WantsCenter:

                SearchSwitchCase(SearchState.Center);

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SearchSwitchCase(SearchState defaultSearch)
    {
        switch (searchState)
        {
            case SearchState.Inactive:
                searchState = defaultSearch;
                break;

            //There is no more plant so behavior is now searching center
            case SearchState.Center:

                alienAnimator.SetBool("Attacking", false);

                if (GoToCenter())
                {
                    navMeshAgent.isStopped = true;
                    searchState = SearchState.AttackCenter;
                }

                break;
            case SearchState.Plant:
                alienAnimator.SetBool("Attacking", false);

                if (currentTargetPlant == null)
                {
                    currentTargetPlant = FindClosestPlant();

                    // No more plants
                    if (currentTargetPlant == null)
                    {
                        Behavior = BehaviorState.WantsCenter;
                        return;
                    }
                }

                if (GoToPlant())
                {
                    // If reached plant
                    navMeshAgent.isStopped = true;
                    searchState = SearchState.AttackPlant;
                }
                break;
            case SearchState.AttackCenter:
                AttackCenter();
                break;
            case SearchState.AttackPlant:
                AttackPlant();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }



    protected virtual bool GoToPlant()
    {
        return GoToPosition(currentTargetPlant);
    }

    private bool GoToCenter()
    {
        return GoToPosition(PlantManager.instance.center.transform.position);
    }

    private Plant FindClosestPlant()
    {
        if (PlantManager.instance.plants.Count == 0)
            return null;

        float closestDistance = float.MaxValue;

        int closeByIndex = 0;
        for (int i = 0; i < PlantManager.instance.plants.Count; i++)
        {
            float distance = Vector3.Distance(PlantManager.instance.plants[i].transform.position, transform.position);
            if (distance <= closestDistance)
            {
                closestDistance = distance;
                closeByIndex = i;
            }
        }

        return PlantManager.instance.plants[closeByIndex];
    }

    public override void OnHit(float damage)
    {
        base.OnHit(damage);
        emmiterHit.Play();
        sprite.GetComponent<SpriteRenderer>().color = Color.red;
    }


    public override void Die()
    {
        base.Die();
        AlienManager.instance.aliens.Remove(this);
        emmitterKill.Play();
        alienAnimator.SetTrigger("Death");
    }

    private void OnTriggerEnter(Collider other)
    {
        // not Already attacking plant or not inactive
        if (searchState != SearchState.Inactive && searchState != SearchState.AttackPlant)
        {
            if (other.CompareTag("Plant"))
            {
                currentTargetPlant = other.GetComponent<Plant>();
                searchState = SearchState.Plant;
            }
        }

        if (other.CompareTag("PeaBall"))
        {
            OnHit(1);
            LeanPool.Despawn(other.gameObject);
        }
    }

    protected virtual void AttackPlant()
    {
        // Plant killed
        if (currentTargetPlant == null || currentTargetPlant.dying)
        {
            navMeshAgent.isStopped = false;
            searchState = SearchState.Inactive;
            return;
        }

        alienAnimator.SetBool("Attacking", true);
        HitEntity(currentTargetPlant);
    }

    private void AttackCenter()
    {

        if (PlantManager.instance.center == null || PlantManager.instance.center.dying)
        {
            return;
        }
        alienAnimator.SetBool("Attacking", true);
        HitEntity(PlantManager.instance.center);
    }

    private void HitEntity(Entity entity)
    {
        currentAttackTime += Time.deltaTime;

        if (currentAttackTime > alienData.damageSpeed)
        {
            currentAttackTime = 0;
            entity.OnHit(alienData.damage);
        }
    }

    private float currentAttackTime = 0;




}
