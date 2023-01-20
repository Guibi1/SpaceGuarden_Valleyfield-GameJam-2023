using Lean.Pool;
using TMPro;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    [SerializeReference] Rigidbody rb;
    [SerializeReference] TextMeshProUGUI text;

    public void SetDamage(float damage)
    {
        text.text = Mathf.RoundToInt(damage).ToString();
    }

    void Start()
    {
        float maxForce = 1500f;
        float minForce = 1000f;
        rb.AddForce(RandomForce(minForce, maxForce), 3000f, RandomForce(minForce, maxForce));

        Color color = text.color;
        StartCoroutine(SimpleRoutines.LerpCoroutine(1f, 0f, 1.4f,
            (opacity) =>
            {
                color.a = opacity;
                text.color = color;
            },
            () => LeanPool.Despawn(this)
        ));
    }

    float RandomForce(float min, float max)
    {
        return (Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(min, max);
    }
}
