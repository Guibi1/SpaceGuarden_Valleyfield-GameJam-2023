using System;
using UnityEngine;
using FMODUnity;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
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
        if (walking)
            emitterWalk.Play();
        else
            emitterWalk.Stop();
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
