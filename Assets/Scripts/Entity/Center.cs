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
            CoinManager.instance.OpenDeath();
        }
    }
}
