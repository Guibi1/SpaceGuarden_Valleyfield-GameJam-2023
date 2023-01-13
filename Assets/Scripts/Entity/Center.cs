using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Center : Entity
{
    public HealthBar hb;

    public void Start()
    {
        hb.maxHealth = HP;
    }

    public void Update()
    {
        hb.currentHealth = HP;
        print("currentHealth " + hb.currentHealth);
        print("maxHealth " + hb.maxHealth);


    }
}
