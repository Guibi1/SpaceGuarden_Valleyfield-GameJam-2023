using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Alien : Agent
{
    public enum BehaviorState {Inactive, WantsPlant, WantsCenter}

    public BehaviorState Behavior ;
    
    public enum SearchState {Inactive, Center, Plant, AttackCenter, AttackPlant}

    public SearchState searchState;
    
    private Plant currentTargetPlant;
    public AlienData alienData;

    private void Update()
    {

        switch (Behavior)
        {
            case BehaviorState.Inactive:
                return;
                break;
            case BehaviorState.WantsPlant:

                switch (searchState)
                {
                    case SearchState.Inactive:
                        searchState = SearchState.Plant;
                        break;
                    
                    //There is no more plant so behavior is now searching center
                    case SearchState.Center:
                        Behavior = BehaviorState.WantsCenter;
                        break;
                    case SearchState.Plant:
                        
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

                break;
            case BehaviorState.WantsCenter:

                if (GoToCenter())
                {
                    AttackCenter();
                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
    }

    private void AttackCenter()
    {
        throw new NotImplementedException();
    }

    private bool GoToPlant()
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
        
        Vector3 closestVector = PlantManager.instance.plants[0].transform.position;
        float closestDistance = Vector3.Distance(PlantManager.instance.plants[0].transform.position, transform.position);

        int closeByIndex = 0;

        for (int i = 1; i < PlantManager.instance.plants.Count; i++)
        {
            float distance = Vector3.Distance(PlantManager.instance.plants[i].transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestVector = PlantManager.instance.plants[i].transform.position;
                closestDistance = distance;
                closeByIndex = i;
            }
        }
        
        return PlantManager.instance.plants[closeByIndex];

    }

    private void OnTriggerEnter(Collider other)
    {
        if (searchState != SearchState.Inactive)
        {
            if (other.CompareTag("Plant"))
            {
                currentTargetPlant = other.GetComponent<Plant>();
                searchState = SearchState.AttackPlant;
            }
        }
    }

    protected virtual void AttackPlant()
    {
        
    }
}
