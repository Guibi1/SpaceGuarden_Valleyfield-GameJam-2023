using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corn : Plant
{
    [SerializeField] private GameObject CornPrefab;
    [SerializeField] private Transform cornSpawnPoint;
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

        GameObject corn = Instantiate(CornPrefab, cornSpawnPoint);
        corn.GetComponent<SingleCorn>().target = closestAlien.gameObject.transform;
        

        
        throw new System.NotImplementedException();
    }

    public override IEnumerator Preparing()
    {
        throw new System.NotImplementedException();
    }

}
