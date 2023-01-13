using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoHitbox : MonoBehaviour
{
    public List<GameObject> aliens = new List<GameObject>();

    public void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.name);
        if (other.CompareTag("Alien"))
        {
            aliens.Add(other.gameObject);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            aliens.Remove(other.gameObject);
        }
    }

    public void DoDamageToEnemies(float damage)
    {
        foreach(var item in aliens)
        {
            if (item == null) aliens.Remove(item);
            item.GetComponent<Alien>().OnHit(damage);
        }
    }
}
