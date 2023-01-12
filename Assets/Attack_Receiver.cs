using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Receiver : MonoBehaviour
{
    [SerializeField] PlayerMouvement player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            player.addAlien(other.GetComponent<Alien>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Alien"))
        {
            player.removeAlien(other.GetComponent<Alien>());
        }
    }

}
