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
        Alien alien = AlienDetector.GetComponent<Alien>();

        if (alien)
        {
            GameObject corn = LeanPool.Spawn(CornPrefab, cornSpawnPoint);
            SingleCorn singleCorn = corn.GetComponent<SingleCorn>(); 
        
            singleCorn.target = alien.gameObject.transform;
            
            print("launch" + alien.name);
        }
            
        
        
        yield return new WaitForEndOfFrame();

    }

    public override IEnumerator Preparing()
    {
        yield return new WaitForEndOfFrame();
    }

}
