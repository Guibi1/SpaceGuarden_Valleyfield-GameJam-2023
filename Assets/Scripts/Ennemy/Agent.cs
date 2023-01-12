using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;

    public void GoToPosition(Vector3 position)
    {
        navMeshAgent.destination = position;
    }
}
