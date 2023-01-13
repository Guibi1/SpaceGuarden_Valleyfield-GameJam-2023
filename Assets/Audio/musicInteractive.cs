using System;
using UnityEngine;
using FMODUnity;

public class musicInteractive : MonoBehaviour
{
    public StudioEventEmitter music;
    private FMOD.Studio.EventInstance instance;
    public EventReference fmodEvent;
    private float enemies, healt;
    
    void Start()
    {
        music.Play();
        InvokeRepeating("checkEnemyStatus", 0f, 2f);
        
        instance = RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    private void Update()
    {
        instance.setParameterByName("Enemies", AlienManager.instance.aliens.Count);
        instance.setParameterByName("Healt", 1);
    }

    void checkEnemyStatus()
    {
        int nbAlien = AlienManager.instance.aliens.Count;
        print("Nb alien: " + nbAlien);
    }
}
