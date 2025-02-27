using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class uses OrbCollectedPanel for showing game explanations. delegates will unsubscribe after hint has been shown for the first time.
/// </summary>
public class ExplanationsUI : MonoBehaviour
{
    const int firstSpellLevel = 3;          // level where player reaches first spell
    [TextArea] public string firstSpellText = "";      // text shown to player when first spell aquired
    [TextArea] public string firstTurretText = "";     // text shown to player when first turret placed
    [TextArea] public string firstBlueBerry = "Mana Regeneration Increased";     // text shown to player when first turret placed
    [TextArea] public string firstOrangeBerry = "Passive Health Regeneration Increased";     // text shown to player when first turret placed


    void Start()
    {
        PlayerExp.Instance.onLevelUp += TryShowSpellInfo;
        PlayerBuild.Instance.onFirstTurretPlaced += TryShowTurretInfo;
        PlayerHealth.Instance.onOrangeBuffUpgrade += TryShowOrangeBuffInfo;
        PlayerMana.Instance.onBlueBuffUpgrade += TryShowBlueBuffInfo;
    }

    private void TryShowOrangeBuffInfo()
    {
        UIManager.Instance.ShowPopupTextAndPause(firstOrangeBerry);
        PlayerHealth.Instance.onOrangeBuffUpgrade -= TryShowOrangeBuffInfo;
    }  
    private void TryShowBlueBuffInfo()
    {
        UIManager.Instance.ShowPopupTextAndPause(firstBlueBerry);
        PlayerMana.Instance.onBlueBuffUpgrade -= TryShowBlueBuffInfo;
    }

    void TryShowTurretInfo()
    {
        UIManager.Instance.ShowPopupTextAndPause(firstTurretText);
        PlayerBuild.Instance.onFirstTurretPlaced -= TryShowTurretInfo;
    }

    void TryShowSpellInfo(Damage.DamageType dmgType)
    {
        int[] levels = PlayerExp.Instance.level;

        if (levels[(int)dmgType] == firstSpellLevel)
        {
            UIManager.Instance.ShowPopupTextAndPause(firstSpellText);
            PlayerExp.Instance.onLevelUp -= TryShowSpellInfo;
            return;
        }
    }

    private void OnDisable()
    {
        if (PlayerExp.Instance != null)
        {
            PlayerExp.Instance.onLevelUp -= TryShowSpellInfo;
        }
    }
}