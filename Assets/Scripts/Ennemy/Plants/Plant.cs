using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class Plant : MonoBehaviour
{
    [SerializeField] private PlantData plantData;
    [SerializeField] private AoEPlant aoEPlant;
    
    protected bool useAnimator;
    [ShowIf("useAnimator")]
    [SerializeField] private Animator animator;
    
    
    
    private Coroutine ActiveRoutine;
    private Coroutine ConstructionRoutine;

    public abstract void Execute();
    public abstract void DrawAoE();
    public abstract void HideAoE();
    
    public enum State {Construction, Active, DeActive}
    private State state;
    public enum AttackState {Idle, Attacking}

    private AttackState attackState;

    [Button]
    public void ChangeState(Plant.State state)
    {
        switch (state)
        {
            case State.Construction:
                
                if (ConstructionRoutine != null)
                {
                    StopCoroutine(RunConstruction());
                }

                ConstructionRoutine = StartCoroutine(RunConstruction());
                
                break;
            case State.Active:
                
                if (ActiveRoutine != null)
                {
                    StopCoroutine(RunActive());
                }

                ActiveRoutine = StartCoroutine(RunActive());
                break;
            case State.DeActive:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    protected virtual IEnumerator RunConstruction()
    {
        yield return new WaitForSeconds(plantData.constructionTime);
        
        ChangeState(State.Active);
    }

    protected virtual IEnumerator RunActive()
    {
        while (state == State.Active)
        {
            attackState = AttackState.Idle;
            yield return new WaitForSeconds(plantData.intervalBetweenExecute);
            
            attackState = AttackState.Attacking;
            if (useAnimator)
            {
                animator.Play("Attack");
            }
            Execute();

            yield return new WaitForSeconds(plantData.executeTime);
        }
    }

}
