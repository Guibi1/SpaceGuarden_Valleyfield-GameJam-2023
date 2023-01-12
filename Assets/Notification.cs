using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeReference] private TextMeshProUGUI text;
    [SerializeReference] private List<Graphic> graphics;
    private Vector3 target;
    private float targetOp;

    private void OnEnable()
    {
        PlayerMouvement.OnNotif += (string text) =>
        {
            target = new Vector3(0, 4, 0);
            targetOp = 1;
            this.text.text = text;
        };

        PlayerMouvement.KillNotif += () =>
        {
            target = new Vector3(0, 0, 0);
            targetOp = 0;
        };
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, 8f * Time.deltaTime);
        foreach (Graphic g in graphics)
        {
            Color color = g.color;
            color.a = Mathf.Lerp(g.color.a, targetOp, Time.deltaTime * 8f);
            g.color = color;
        }


        transform.rotation = GameObject.FindObjectOfType<CinemachineFreeLook>().transform.localRotation;
    }
}
