using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Alien : Agent
{
    public enum BehaviorState {Inactive, WantsTower, WantsCenter}

    public BehaviorState Behavior ;
    
    public enum AttackState { Inactive, Center, Tower}

    public AttackState attackState;

    private void Update()
    {

        switch (Behavior)
        {
            case BehaviorState.Inactive:
                return;
                break;
            case BehaviorState.WantsTower:

                switch (expression)
                {
                    
                }
                
                GoToPosition(EnnemyManager.instance.PlantPosition.position);
                break;
            case BehaviorState.WantsCenter:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            attackState = AttackState.Tower;
        }
    }
}
