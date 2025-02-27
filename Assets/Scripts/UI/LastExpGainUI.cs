using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastExpGainUI : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        PlayerExp.Instance.onGetExp += TryPlayExpGainAnim;
        PlayerExp.Instance.onLevelUp += TryPlayLvlUpAnim;
    }

    private void TryPlayLvlUpAnim(Damage.DamageType dmgType)
    {
        if (PlayerExp.Instance.level[(int)dmgType] == 3)
        {
            anim.Play("Level3");
        }
        else
        {
            anim.Play("LevelUp");
        }
    }

    private void TryPlayExpGainAnim()
    {
        if (PlayerExp.Instance.lastExpType != PlayerExp.ExpType.None)
        {
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Level3") && !anim.GetCurrentAnimatorStateInfo(0).IsName("LevelUp"))
            {
                anim.Play("ExpGain");
            }
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        if (PlayerExp.Instance != null)
        {
            PlayerExp.Instance.onGetExp -= TryPlayExpGainAnim;
            PlayerExp.Instance.onLevelUp -= TryPlayLvlUpAnim;
        }
    }
}