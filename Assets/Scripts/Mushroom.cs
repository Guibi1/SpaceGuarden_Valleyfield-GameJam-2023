using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class Mushroom : Plant
{
    public MushroomHitbox mhb;
    public StudioEventEmitter emitterAttack;
    public override IEnumerator Execute()
    {
        emitterAttack.Play();
        mhb.DoDamageToEnemies(plantData.damage);
        yield return null;
    }

    public override IEnumerator Preparing()
    {
        yield return null;
    }
}
