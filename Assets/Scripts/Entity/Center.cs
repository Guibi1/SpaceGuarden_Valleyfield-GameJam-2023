using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Center : Entity
{
    public float maxHealth;

    private void OnEnable()
    {
        maxHealth = HP;
    }

    public override void OnHit(float damage, Object triggerer)
    {
        base.OnHit(damage, triggerer);
        Debug.Log("on hit");
        if (dying)
        {
            Time.timeScale = 0; // Game end
            CoinManager.instance.OpenDeath();
        }
    }
}
