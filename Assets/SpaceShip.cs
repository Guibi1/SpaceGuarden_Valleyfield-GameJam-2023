using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Sirenix.OdinInspector;

public class SpaceShip : MonoBehaviour
{
    public static SpaceShip instance;


    public float speed = 2f;
    public float noiseScale = 1.0f;
    public float noiseStrength = 1.0f;

    private bool goUp;

    public Transform place;

    public StudioEventEmitter emitterup;

    private float ok;

    [SerializeField]
    private Transform SpawnPoint;

    private void Start()
    {
        instance = this;
        ok = transform.position.y + 50f;
    }

    void Update()
    {
        if (goUp)
        {
            if (Vector3.Distance(transform.position, new Vector3(transform.position.x, ok, transform.position.z)) < 0.1f)
            {
                emitterup.Stop();
                return;
            }
            float yPos = Mathf.PerlinNoise(Time.time * noiseScale, 0.0f) * noiseStrength;
            transform.position += new Vector3(0.0f, yPos * speed * Time.deltaTime, 0.0f);
        }
        else
        {
            if (Vector3.Distance(transform.position, place.transform.position) < 0.1f)
            {
                emitterup.Stop();
                return;
            }


            float yPos = Mathf.PerlinNoise(Time.time * noiseScale, 0.0f) * noiseStrength;
            transform.position -= new Vector3(0.0f, yPos * speed * Time.deltaTime * 2f, 0.0f);


        }


    }


    public void PickUp()
    {
        if (reference != null)
        {
            PlayerMouvement.instance.PickUpPlant(reference);
            Destroy(reference);
        }
    }

    public Plant reference;

    [Button]
    public void GoDown(Plant plant)
    {
        // le texte
        reference = Instantiate(plant, SpawnPoint.transform.position, Quaternion.identity);
        goUp = false;
    }

    [Button]
    public void GoUp()
    {
        emitterup.Play();
        goUp = true;
    }
}
