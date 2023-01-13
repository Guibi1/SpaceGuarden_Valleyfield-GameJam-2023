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
        
        instance = FMODUnity.RuntimeManager.CreateInstance(fmodEvent);
        instance.start();
        
        InvokeRepeating("checkEnemyStatus", 0f, 2f);
    }

    void checkEnemyStatus()
    {
        int nbAlien = AlienManager.instance.aliens.Count;
        
        instance.setParameterByName("Enemies", nbAlien);
        instance.setParameterByName("Healt", 1);
    }
}
