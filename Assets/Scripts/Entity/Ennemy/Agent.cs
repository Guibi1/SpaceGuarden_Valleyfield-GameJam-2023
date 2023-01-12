using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : Entity
{
    [SerializeField] protected NavMeshAgent navMeshAgent;
    public float detectPlantThreshold;
    public float detectCenterThresold;
    
    public bool GoToPosition(Vector3 position)
    {
        navMeshAgent.destination = position;
        
        if (navMeshAgent.remainingDistance < detectCenterThresold)
        {
            return true;
        }

        return false;
    }

    public bool GoToPosition(Plant plant)
    {
        navMeshAgent.destination = plant.transform.position;

        
        if (navMeshAgent.remainingDistance < detectPlantThreshold)
        {
            return true;
        }
        
        return false;
    }
}
