using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float HP;
    public bool allowPooling;
    public bool deathAnimation;

    [ShowIf("deathAnimation")]
    public float deathAnimationTime;

    public bool dying;

    public void OnHit(float damage)
    {
        if (dying)
            return;


        if (HP - damage <= 0)
        {
            damage = HP;
            StartCoroutine(Die());
            dying = true;
        }

        HP -= damage;
    }

    public IEnumerator Die()
    {
        if (deathAnimation)
            yield return new WaitForSeconds(deathAnimationTime);

        if (allowPooling)
            LeanPool.Despawn(gameObject);
        else
            Destroy(gameObject);
    }
}
