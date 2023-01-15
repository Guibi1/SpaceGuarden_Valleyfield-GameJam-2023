using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Sirenix.OdinInspector;
using Lean.Pool;

public class SpaceShip : MonoBehaviour
{
    public static SpaceShip instance;


    public float speed = 2f;
    public float noiseScale = 1.0f;
    public float noiseStrength = 1.0f;

    private bool goUp;

    public StudioEventEmitter emitterup;

    private float initialY;

    [SerializeField]
    private Transform SpawnPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        if (goUp)
        {
            if (transform.position.y > initialY + 25f)
            {
                emitterup.Stop();
                return;
            }

            float yPos = Mathf.PerlinNoise(Time.time * noiseScale, 0.0f) * noiseStrength;
            transform.position += new Vector3(0.0f, yPos * speed * Time.deltaTime, 0.0f);
        }
        else
        {
            if (transform.position.y < initialY)
            {
                emitterup.Stop();
                return;
            }

            float yPos = Mathf.PerlinNoise(Time.time * noiseScale, 0.0f) * noiseStrength;
            transform.position -= new Vector3(0.0f, yPos * speed * Time.deltaTime * 3f, 0.0f);
        }
    }


    public void PickUp()
    {
        PlayerMouvement.instance.PickUpPlant(BaseCampManager.instance.nextShippement);
        Destroy(referenceVaisseau.gameObject);
    }

    public Plant referenceVaisseau;

    [Button]
    public void GoDown(Plant plant)
    {
        referenceVaisseau = LeanPool.Spawn(plant, SpawnPoint.transform.position, Quaternion.identity, transform);
        goUp = false;
    }

    [Button]
    public void GoUp()
    {
        emitterup.Play();
        goUp = true;
    }
}
