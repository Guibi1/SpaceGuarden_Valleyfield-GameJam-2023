using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Mushroom : Plant
{
    public MushroomHitbox mhb; 
    public override IEnumerator Execute()
    {
        mhb.DoDamageToEnemies(plantData.damage);
        yield return null;
    }

    public override IEnumerator Preparing()
    {
        yield return null;
    }
}
