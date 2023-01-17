using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SplashText : MonoBehaviour
{
    string[] poss = {
    "Un concept totalement original!",
    "Fait en 48 heures!",
    "100% home-made",
    "Il y a des *bugs* dans ce jeu ",
    "Un des jeux de tous les temps",
    "Certifié 'un bon TP!'",
    "#1 Au GameJam valleyfield!",
    "Des créateurs de Space Ship",
    "Un jeu de 'jardinage'",
    "Oh mon dieu c'est Gilles",
    "Supporté par Mr. Brochure"
    };
    float x = 0;


    private void Start()
    {
        Time.timeScale = 1;
        GetComponent<TextMeshProUGUI>().text = poss[Random.Range(0, poss.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        x += Time.deltaTime * 2;
        float y = (Mathf.Sin(x) / 8 + 2) / 2;
        transform.localScale = new Vector3(y, y, y);
    }
}
