using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlugAlien : Alien
{
    public override void OnHit(float damage)
    {
        Debug.Log("Hello, world!");
        base.OnHit(damage);
        
    }


}
