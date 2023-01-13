using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using FMODUnity;

public abstract class Plant : Entity
{
    [SerializeField] public PlantData plantData;
    [SerializeField] protected AoEPlant aoEPlant;


    protected bool useAnimator;
    [ShowIf("useAnimator")]
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject healArea;

    private Coroutine ActiveRoutine;
    private Coroutine ConstructionRoutine;

    public abstract IEnumerator Execute();
    public abstract IEnumerator Preparing();

    //Sound
    public StudioEventEmitter emitterKill;


    public override void Start()
    {
        healthBar.maxHealth = plantData.health;
        HP = plantData.health;

        if (healArea != null)
        {
            SetHealth(plantData.health);
            LeanPool.Spawn(healArea, transform).transform.localPosition = Vector3.zero;
        }

        ChangeState(State.Active);
    }

    public void SetHealth(float health)
    {
        HP = Mathf.Clamp(health, 0f, plantData.health);
    }


    [Button]
    public void TestExecute()
    {
        StartCoroutine(Execute());
    }

    [Button]
    public void TestPrepare()
    {
        StartCoroutine(Preparing());
    }

    public virtual void DrawAoE()
    {
        aoEPlant.AlphaAppear();
    }

    public virtual void HideAoE()
    {
        aoEPlant.AlphaDisapear();
    }

    public enum State { Construction, Active, DeActive }

    [ShowInInspector]
    [ReadOnly]
    private State state;
    public enum AttackState { Idle, Attacking }

    private AttackState attackState;

    [Button]
    public void ChangeState(Plant.State state)
    {
        this.state = state;

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


    public override void Die()
    {
        PlantManager.instance.plants.Remove(this);
        emitterKill.Play();
        base.Die();
    }

    protected virtual IEnumerator RunConstruction()
    {
        yield return new WaitForSeconds(plantData.constructionTime);
        ChangeState(State.Active);
    }

    protected virtual IEnumerator RunActive()
    {

        yield return new WaitForEndOfFrame();
        while (state == State.Active)
        {
            attackState = AttackState.Idle;
            yield return new WaitForSeconds(plantData.intervalBetweenExecute);

            attackState = AttackState.Attacking;

            if (useAnimator)
                animator.Play("Attack");

            StartCoroutine(Execute());

            yield return new WaitForSeconds(plantData.executeTime);

            StartCoroutine(Preparing());
        }
    }

}
