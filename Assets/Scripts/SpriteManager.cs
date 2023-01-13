using System;
using UnityEngine;
using FMODUnity;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    private bool _lastStatus;
    public Animator animator;

    public StudioEventEmitter emitterWalk;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetBald(OptionsManager.instance.Bald);
        SetCharType(PlayerTypes.Scythe);
    }

    public void SetWalking(bool walking)
    {
        animator.SetBool("IsWalking", walking);

        if (walking && walking != _lastStatus)
        {
            _lastStatus = true;
            emitterWalk.Play();
        }
        else if (walking != _lastStatus)
        {
            _lastStatus = false;
            emitterWalk.Stop();
        }
    }

    public void SetBald(bool bald)
    {
        animator.SetBool("IsBald", bald);
    }

    public void SetCharType(PlayerTypes type)
    {
        animator.SetTrigger("ResetAnim");
        animator.SetInteger("Type", (int)type);
    }

    internal void attack()
    {
        animator.SetTrigger("Attack");
    }
}
