using System;
using System.Collections;
using Sirenix.Utilities;
using UnityEngine;
using FMODUnity;

public class Tomato : Plant
{
    public GameObject TopPart;
    public GameObject tomato;
    public float height;
    public float bottom;

    public ParticleSystem particle1;
    public ParticleSystem particle2;
    public ParticleSystem particle3;
    public GameObject[] tomatoes;

    private void OnEnable()
    {
        bottom = TopPart.transform.position.y;
    }

    public AnimationCurve curveCrush;
    public AnimationCurve curveUp;
    public TomatoHitbox hitbox;
    public StudioEventEmitter emitterAttack;

    public override IEnumerator Execute()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            TopPart.transform.localPosition = new Vector3(TopPart.transform.localPosition.x, f, TopPart.transform.localPosition.z);
            tomatoes.ForEach(x => x.SetActive(false));
            int pos = (int)(15 - Math.Floor((TopPart.transform.localPosition.y + 0.8f) * 7));
            if (pos < tomatoes.Length)
            {
                tomatoes[pos].SetActive(true);
            }

        }, 1.42f, 0.5f, plantData.executeTime, curveCrush));

        StartCoroutine(SimpleRoutines.WaitTime(plantData.executeTime, () =>
        {
            particle1.Play();
            particle2.Play();
            particle3.Play();
            emitterAttack.Play();
            
            hitbox.DoDamageToEnemies(plantData.damage);
        }));



    }

    public override IEnumerator Preparing()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {

            TopPart.transform.localPosition = new Vector3(TopPart.transform.localPosition.x, f, TopPart.transform.localPosition.z);
            tomatoes.ForEach(x => x.SetActive(false));

            int pos = (int)(15 - Math.Floor((TopPart.transform.localPosition.y + 0.8f) * 7));            
            if (pos < tomatoes.Length)
            {
                tomatoes[pos].SetActive(true);
            }
        }, 0.5f, 1.42f, plantData.preparingTime, curveUp));
    }
}
