using System.Collections;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] public float HP;
    [SerializeField] public bool allowPooling;
    [SerializeReference] SplashDamage splashDamagePrefab;
    [SerializeField] public bool deathAnimation;

    [ShowIf("deathAnimation")]
    [SerializeField] public float deathAnimationTime;

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

        if (damage > 0)
        {
            if(s  )
            LeanPool.Spawn(splashDamagePrefab, transform).SetDamage(damage);
        }
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
