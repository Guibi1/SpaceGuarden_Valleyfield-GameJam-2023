using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pea : Plant
{
    public Alien aimedAlien;
    public GameObject Turret;
    
    public override IEnumerator Execute()
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator Preparing()
    {
        throw new System.NotImplementedException();
    }

    private void Update()
    {
        if (aimedAlien == null)
            return;
        
       // Turret.transform.LookAt(aimedAlien);

        if (aimedAlien.dying)
        {
            aimedAlien = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (aimedAlien != null)
            return;
        
        if (other.CompareTag("Alien"))
        {

            aimedAlien = aimedAlien.GetComponent<Alien>();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (aimedAlien != null)
            return;
        
        if (other.CompareTag("Alien"))
        {
            aimedAlien = aimedAlien.GetComponent<Alien>();
        }
    }
}
