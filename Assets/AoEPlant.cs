using System.Collections;
using System.Collections.Generic;
using Shapes;
using Sirenix.OdinInspector;
using UnityEngine;

public class AoEPlant : MonoBehaviour
{
    public List<Shapes.Disc> DiscRenderers;
    public float time;

    public Coroutine FadeAppear;
    public Coroutine FadeDisapear;

    [Button]
    public void AlphaAppear()
    {
        if (FadeDisapear != null)
        {
            StopCoroutine(FadeDisapear);
            FadeDisapear = null;
        }
        
        if (FadeAppear == null)
        {
            FadeAppear = StartCoroutine(FadeShapeRenderers(1));
        }
    }

    [Button]
    public void AlphaDisapear()
    {
        if (FadeAppear != null)
        {
            StopCoroutine(FadeAppear);
            FadeAppear = null;
        }

        if (FadeDisapear == null)
        {
            FadeDisapear = StartCoroutine(FadeShapeRenderers(0));
        }
    }

    private IEnumerator FadeShapeRenderers(float alpha)
    {
        FadeDisapear = null;

        yield return SimpleRoutines.LerpCoroutine(DiscRenderers[0].ColorInner.a, alpha, time, (f =>
        {
            DiscRenderers.ForEach(d =>
            {
                d.ColorInner = new Color(d.ColorInner.r, d.ColorInner.g, d.ColorInner.b, f);
                d.ColorOuter = new Color(d.ColorOuter.r, d.ColorOuter.g, d.ColorOuter.b, f);
            });
        }));
    }

    public void UpdateRadius(float radius)
    {
        DiscRenderers.ForEach(d=>d.Radius = radius);
    }
}
