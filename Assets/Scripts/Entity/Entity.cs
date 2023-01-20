using System.Collections;
using Lean.Pool;
using Sirenix.OdinInspector;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float _healthPoint;

    public virtual float HP
    {
        get => _healthPoint;
        set
        {
            _healthPoint = value;

            if (healthBar != null)
            {
                healthBar.currentHealth = value;
            }
        }
    }
    [SerializeField] public bool allowPooling;
    [SerializeReference] protected HealthBar healthBar;
    [SerializeReference] SplashDamage splashDamagePrefab;
    [SerializeField] public float splashDamageY = 1f;
    [SerializeField] public bool deathAnimation;

    [ShowIf("deathAnimation")]
    [SerializeField] public float deathAnimationTime;

    public bool dying;

    public virtual void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxHealth = HP;
        }
    }

    public virtual void OnHit(float damage, Object triggerer)
    {
        print("Victim : " + name + ". Triggerer :" + triggerer.name);
        if (dying)
            return;

        if (HP - damage <= 0)
        {
            damage = HP;
            Die();
            dying = true;
        }

        HP -= damage;

        if (damage > 0)
        {
            if (splashDamagePrefab != null)
            {
                LeanPool.Spawn(splashDamagePrefab, transform.position + new Vector3(0f, splashDamageY, 0f), Quaternion.identity, transform).SetDamage(damage);
            }
        }
    }

    public virtual void Die()
    {
        CoinManager.instance.GainCoins(1);
        StartCoroutine(DieRoutine());
    }

    public IEnumerator DieRoutine()
    {
        if (deathAnimation)
            yield return new WaitForSeconds(deathAnimationTime);

        if (allowPooling)
            LeanPool.Despawn(gameObject);
        else
            Destroy(gameObject);
    }
}
