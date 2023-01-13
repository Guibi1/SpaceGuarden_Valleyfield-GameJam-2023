using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using static UnityEngine.Rendering.DebugUI;

public class Cherry : Plant
{
    public AnimationCurve curveCrush;
    public GameObject axis;
    public GameObject CherryPrefab;
    public GameObject spawnpoint;
    public override IEnumerator Execute()
    {
        spawnpoint.SetActive(true);

        yield return new WaitForEndOfFrame();
        StartCoroutine(SimpleRoutines.CustomCurveLerpCoroutine((f) =>
        {
            axis.transform.localRotation = Quaternion.Euler(new Vector3(axis.transform.localEulerAngles.x, axis.transform.localEulerAngles.y, -f));
        }, 0, 50, plantData.executeTime, curveCrush));

        StartCoroutine(SimpleRoutines.WaitTime(plantData.executeTime, () =>
        {
            spawnpoint.SetActive(false);
            GameObject cherry = LeanPool.Spawn(CherryPrefab);
            cherry.transform.position = spawnpoint.transform.position;
            cherry.transform.rotation = spawnpoint.transform.rotation;

            cherry.GetComponent<Rigidbody>().velocity = new Vector3(10, 8, 0);
        }));


        yield return null;
    }

    public override IEnumerator Preparing()
    {
        yield return null;
    }

}
