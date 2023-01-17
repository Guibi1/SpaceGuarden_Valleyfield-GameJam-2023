using System;
using UnityEngine;
using FMOD.Studio;
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
        instance = RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
    }

    private void OnDestroy()
    {
        music.Stop();
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }

    private void Update()
    {
        instance.setParameterByName("Enemies", AlienManager.instance.aliens.Count);
        instance.setParameterByName("Healt", 1);
    }
}
