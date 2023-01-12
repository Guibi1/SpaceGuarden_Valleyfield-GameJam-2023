using Lean.Pool;
using TMPro;
using UnityEngine;

public class SplashDamage : MonoBehaviour
{
    [SerializeReference] Camera cam;
    [SerializeReference] Rigidbody rb;
    [SerializeReference] TextMeshProUGUI text;

    public void SetDamage(float damage)
    {
        text.text = Mathf.RoundToInt(damage).ToString();
    }

    void Start()
    {
        float maxForce = 2500f;
        float minForce = 2000f;
        rb.AddForce(RandomForce(minForce, maxForce), 5000f, RandomForce(minForce, maxForce));

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

    void Update()
    {
        transform.LookAt(cam.transform);
    }

    float RandomForce(float min, float max)
    {
        return (Random.Range(0, 2) == 0 ? 1 : -1) * Random.Range(min, max);
    }
}
