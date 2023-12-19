using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battler : MonoBehaviour
{
    public BattleChar chara;
    public Animator anim;
    public void SetAnimatorTrigger(string t)
    {
        anim.SetTrigger(t);
    }

    public bool IsCharDeathAnimationActive()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Death") return true;
        else return false;
    }
}
