using System.Collections.Generic;
using UnityEngine;

public class AlienManager : MonoBehaviour
{
    public static AlienManager instance;

    public List<Alien> aliens = new List<Alien>();
    public int killedAlien;
    public int spawnedAlien;

    private void Awake()
    {
        instance = this;
    }
}
