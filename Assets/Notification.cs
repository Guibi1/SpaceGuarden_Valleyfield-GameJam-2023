using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [SerializeReference] private float visibleY = 5f;
    [SerializeReference] private TextMeshProUGUI text;
    [SerializeReference] private List<Graphic> graphics;

    private Vector3 targetPosition;
    private float targetOpacity;

    public void ShowText(string messgae)
    {
        targetPosition = new Vector3(0, visibleY, 0);
        targetOpacity = 1;
        text.text = messgae;
    }

    public void HideText()
    {
        targetPosition = new Vector3(0, 0, 0);
        targetOpacity = 0;
    }

    private void OnEnable()
    {
        // PlayerMouvement.OnNotif += ShowText;
        // PlayerMouvement.KillNotif += HideText;
    }

    private void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, 8f * Time.deltaTime);
        foreach (Graphic g in graphics)
        {
            Color color = g.color;
            color.a = Mathf.Lerp(g.color.a, targetOpacity, Time.deltaTime * 8f);
            g.color = color;
        }

        transform.rotation = GameObject.FindObjectOfType<CinemachineFreeLook>().transform.localRotation;
    }
}
