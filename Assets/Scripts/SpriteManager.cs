using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour
{
    public static SpriteManager instance;
    public Animator animator;
    
    private void Start()
    {

        SetCharType(PlayerTypes.Scythe_Bald);
        instance = this;
    }
    public void SetWalking(bool walking)
    {
        animator.SetBool("Is_Walking", walking);
    }

    public void SetCharType(int type)
    {
        animator.SetInteger("Type", type);
    }

    public void SetCharType(PlayerTypes type)
    {
        animator.SetTrigger("ResetAnim");
        animator.SetInteger("Type", (int) type);
    }
}
