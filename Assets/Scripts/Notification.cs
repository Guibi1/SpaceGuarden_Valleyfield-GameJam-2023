using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private float visibleY = 5f;
    [SerializeField]
    private float animationTime = .5f;
    [SerializeReference] private List<Graphic> graphics;

    private float initialY;
    private Vector3 targetPosition;
    private bool hidden = true;

    void Start()
    {
        initialY = transform.localPosition.y;
        gameObject.SetActive(false);
        HideText();
    }

    public void ShowText(string message)
    {
        if (hidden)
        {
            StopAllCoroutines();
            gameObject.SetActive(true);
            StartCoroutine(SimpleRoutines.LerpCoroutine(transform.localPosition.y, visibleY, animationTime, (y) => transform.localPosition = new Vector3(0, y, 0)));
            StartCoroutine(SimpleRoutines.LerpCoroutine(graphics[0].color.a, 1, animationTime, (a) =>
            {
                foreach (Graphic g in graphics)
                {
                    Color color = g.color;
                    g.color = color;
                }
            }));
        }

        GetComponentInChildren<TextMeshProUGUI>().text = message;
        hidden = false;
    }

    public void HideText()
    {
        if (!hidden)
        {
            StopAllCoroutines();
            StartCoroutine(SimpleRoutines.LerpCoroutine(transform.localPosition.y, initialY, animationTime, (y) => transform.localPosition = new Vector3(0, y, 0)));
            StartCoroutine(SimpleRoutines.LerpCoroutine(graphics[0].color.a, 0, animationTime, (a) =>
            {
                foreach (Graphic g in graphics)
                {
                    Color color = g.color;
                    g.color = color;
                }
            }, () => gameObject.SetActive(false)));
        }

        hidden = true;
    }

    private void FixedUpdate()
    {
        // transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 8f * Time.deltaTime);


        // enabled = hide && graphics[0].color.a < .01f;

        // transform.rotation = GameObject.FindObjectOfType<CinemachineFreeLook>().transform.localRotation;
    }
}
