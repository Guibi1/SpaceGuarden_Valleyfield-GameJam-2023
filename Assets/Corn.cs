using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class Corn : Plant
{
    [SerializeField] private GameObject CornPrefab;
    [SerializeField] private Transform cornSpawnPoint;
    public AlienDetector AlienDetector;
    public override IEnumerator Execute()
    {
        float closest = Mathf.Infinity;
        Alien closestAlien = null;
        foreach(Alien a in AlienManager.instance.aliens)
        {
            if(Vector3.Distance(transform.position, a.transform.position) < closest)
            {
                closest = Vector3.Distance(transform.position, a.transform.position);
                closestAlien = a;
            }
        }

        if (closestAlien == null)
            yield break;
            
        GameObject corn = LeanPool.Spawn(CornPrefab, cornSpawnPoint);
        SingleCorn singleCorn = corn.GetComponent<SingleCorn>(); 
        
        singleCorn.target = closestAlien.gameObject.transform;
        
        
        yield return new WaitForEndOfFrame();

    }

    public override IEnumerator Preparing()
    {
        yield return new WaitForEndOfFrame();
    }

}
