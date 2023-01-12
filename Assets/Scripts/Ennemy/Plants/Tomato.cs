using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Tomato : Plant
{
    public GameObject TopPart;
    public GameObject tomato;
    public float height;
    public float bottom;
    public float tomatoSize;

    private void OnEnable()
    {
        bottom = TopPart.transform.position.y;
    }

    public AnimationCurve curveCrush;
    public AnimationCurve curveUp;
    
    public override IEnumerator Execute()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            TopPart.transform.position = new Vector3(TopPart.transform.position.x, f, TopPart.transform.position.z);
        }, TopPart.transform.position.y, bottom, plantData.executeTime, curveCrush));

        
    }
    
    public override IEnumerator Preparing()
    {
        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            TopPart.transform.position = new Vector3(TopPart.transform.position.x, f, TopPart.transform.position.z);
        }, TopPart.transform.position.y, bottom + height, plantData.preparingTime, curveUp));
    }
}
