using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : Entity
{


    public override void OnHit(float damage)
    {
        base.OnHit(damage);

        if (dying)
        {
            Time.timeScale = 0; // Game end
            CoinManager.instance.OpenDeath();
        }
    }
}
