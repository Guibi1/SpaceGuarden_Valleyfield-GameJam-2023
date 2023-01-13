using Shapes;
using System;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeReference] public Line healthLine;
    [SerializeReference] public Line hitLine;
    [SerializeField] public Color lowHealthColor;
    private Color healthColor;

    public float maxHealth { get; set; } = 100;

    private float _currentHealth = 100;
    public float currentHealth
    {
        get => _currentHealth;
        set
        {
            try
            {
                float endX = 1f * value / maxHealth - 0.5f;
                healthLine.End = new Vector3(endX, 0);
                StartCoroutine(SimpleRoutines.WaitTime(.2f, () => StartCoroutine(SimpleRoutines.LerpCoroutine(hitLine.End.x, endX, .4f, (x) => hitLine.End = new Vector3(x, 0)))));

                bool wasLow = _currentHealth / maxHealth <= .2f;
                bool isLow = value / maxHealth <= .2f;

                if (!wasLow && isLow)
                {
                    StartCoroutine(SimpleRoutines.LerpCoroutine(0, 1, .3f, (t) => healthLine.Color = Color.Lerp(healthColor, lowHealthColor, t)));
                }
                else if (wasLow && !isLow)
                {
                    StartCoroutine(SimpleRoutines.LerpCoroutine(0, 1, .3f, (t) => healthLine.Color = Color.Lerp(lowHealthColor, healthColor, t)));
                }
                if (value == 0)
                {

                }
                _currentHealth = value;
            }
            catch (Exception)
            {
                print("Object was killed.");
            }
        }
    }

    void Start()
    {
        healthColor = healthLine.Color;
    }

}
