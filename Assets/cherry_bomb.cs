using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cherry_bomb : MonoBehaviour
{
    public GameObject cherry_explosion;
    private void OnTriggerEnter(Collider collision)
    {
        print(collision.gameObject.tag);
        if (!collision.CompareTag("Alien") && !collision.CompareTag("Floor")) return;
        if (collision.CompareTag("Alien"))
        {
            collision.gameObject.GetComponent<Alien>().OnHit(40);
        }
        GameObject anim = LeanPool.Spawn(cherry_explosion);
        anim.transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        Destroy(anim, 2f);


        Destroy(gameObject);

    }
    
}
