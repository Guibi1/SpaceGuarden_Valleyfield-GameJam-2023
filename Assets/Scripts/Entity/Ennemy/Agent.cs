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
        if (navMeshAgent.remainingDistance < detectCenterThresold)
        {
            return true;
        }
        
        navMeshAgent.destination = position;
        return false;
    }

    public bool GoToPosition(Plant plant)
    {
        if (navMeshAgent.remainingDistance < detectPlantThreshold)
        {
            return true;
        }
        
        navMeshAgent.destination = plant.transform.position;
        return false;
    }
}
