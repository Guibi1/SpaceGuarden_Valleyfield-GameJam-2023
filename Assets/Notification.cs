using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Vector3 target;
    private float targetOp;
    public List<Graphic> notification;
    private void OnEnable()
    {
        PlayerMouvement.OnNotif += (string text) =>
        {
            target = new Vector3(0, 10, 0);
            targetOp = 1;
            this.text.text = text;
        };

        PlayerMouvement.KillNotif += () => {
            target = new Vector3(0, 0, 0);
            targetOp = 0;
        };


    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, target, 8f * Time.deltaTime);
        foreach (Graphic g in notification)
        {
            g.color  = new Color(g.color.r, g.color.g, g.color.b, Mathf.Lerp(g.color.a, targetOp, Time.deltaTime * 8f));
        }


        transform.rotation = GameObject.FindObjectOfType<CinemachineFreeLook>().transform.localRotation;

    }

}
