using System;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    public Animator animator;

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
