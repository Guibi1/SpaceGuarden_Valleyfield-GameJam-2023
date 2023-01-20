using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class peaball : MonoBehaviour
{
    void Start()
    {
        LeanPool.Despawn(gameObject, 7f);
    }

} // Update is called once per frame

