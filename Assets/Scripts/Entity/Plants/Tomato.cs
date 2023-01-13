using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class Tomato : Plant
{

    public GameObject TopPart;
    public GameObject tomato;
    public float height;
    public float bottom;
    public float damage;

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
    
    public override IEnumerator Execute()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            TopPart.transform.position = new Vector3(TopPart.transform.position.x, f, TopPart.transform.position.z);
            tomatoes.ForEach(x => x.SetActive(false));
            int pos = (int)(8 - Math.Floor((TopPart.transform.position.y + 0.8f) * 8));
            if (pos < tomatoes.Length)
            {
                tomatoes[pos].SetActive(true);
            }

        }, TopPart.transform.position.y, bottom, plantData.executeTime, curveCrush));

        StartCoroutine(SimpleRoutines.WaitTime(plantData.executeTime, () =>
        {
            particle1.Play();
            particle2.Play();
            particle3.Play();
            hitbox.DoDamageToEnemies(damage);
        }));



    }
    
    public override IEnumerator Preparing()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            TopPart.transform.position = new Vector3(TopPart.transform.position.x, f, TopPart.transform.position.z);
            tomatoes.ForEach(x => x.SetActive(false));
            int pos = (int)(8 - Math.Floor((TopPart.transform.position.y + 0.8f) * 8));
            if (pos < tomatoes.Length)
            {
                tomatoes[pos].SetActive(true);
            }
        }, TopPart.transform.position.y, bottom + height, plantData.preparingTime, curveUp));
    }
}
