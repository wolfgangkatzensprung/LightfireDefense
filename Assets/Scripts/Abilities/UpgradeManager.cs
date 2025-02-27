using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeManager : Singleton<UpgradeManager>
{
    const int dictLength = 6;
    Dictionary<UpgradeType, int> upgradesDictionary = new Dictionary<UpgradeType, int>(dictLength);

    public Transform upgradeButtonHolder;

    public delegate void UpgradeDelegate(UpgradeType attr);
    public UpgradeDelegate onUpgrade;

    public override void Awaken()
    {
        ResetUpgradeDictionary();
    }

    public enum UpgradeType
    {
        Turret = 0,
        LighthouseHp = 1,   // max Hp
        LighthouseDmg = 2,
        LighthouseRange = 3,
        PlayerHp = 4,
        PlayerMana = 5
    }

    internal void LoadUpgrades(int[] upgrades)
    {
        UpdateLighthouseHp(upgrades[1]);
        UpdateLighthouseDmg(upgrades[2]);
        UpdateLighthouseRange(upgrades[3]);
        UpdatePlayerHealth(upgrades[4]);
        UpdatePlayerMana(upgrades[5]);

        for (int i = 0; i < upgrades.Length; i++)
        {
            upgradesDictionary[(UpgradeType)i] = upgrades[i];
        }
    }

    internal void ResetUpgrades()
    {
        ResetUpgradeDictionary();
        LighthouseManager.Instance.SetDefaults();
    }

    internal void Upgrade(UpgradeType upgradeAttribute)     // Upgrade ohne UI
    {
        if (upgradesDictionary.TryGetValue(upgradeAttribute, out int attributeLevel))
        {
            upgradesDictionary[upgradeAttribute] = attributeLevel + 1;
        }
        else
        {
            upgradesDictionary.Add(upgradeAttribute, 1);
        }

        IncreaseAttributeStrength(upgradeAttribute);

        onUpgrade?.Invoke(upgradeAttribute);
    }

    internal void Upgrade(UpgradeType upgradeAttribute, TextMeshProUGUI attributeLevelText)     // Upgrade mit UI
    {
        if(upgradesDictionary.TryGetValue(upgradeAttribute, out int attributeLevel))
        {
            upgradesDictionary[upgradeAttribute] = attributeLevel + 1;
        }
        else
        {
            upgradesDictionary.Add(upgradeAttribute, 1);
        }

        IncreaseAttributeStrength(upgradeAttribute);

        UpdateAttributeLevelText(attributeLevelText, upgradesDictionary[upgradeAttribute]);

        onUpgrade?.Invoke(upgradeAttribute);
    }

    internal int[] GetUpgrades()
    {
        return new int[]
        {
            upgradesDictionary[UpgradeType.Turret],
            upgradesDictionary[UpgradeType.LighthouseHp],
            upgradesDictionary[UpgradeType.LighthouseDmg],
            upgradesDictionary[UpgradeType.LighthouseRange],
            upgradesDictionary[UpgradeType.PlayerHp],
            upgradesDictionary[UpgradeType.PlayerMana]
        };
    }

    private void IncreaseAttributeStrength(UpgradeType atr)
    {
        ApplyUpgradeSwitch(atr);
        // sonstige verbesserungen

    }

    private void ApplyUpgradeSwitch(UpgradeType uType)
    {
        int upgradeIndex = (int)uType;
        int upgradeValue = upgradesDictionary[uType];

        switch(upgradeIndex)
        {
            case 0: //turret
                break;
            case 1: //lh hp                
                UpdateLighthouseHp(upgradesDictionary[uType]);
                break;
            case 2: //lh dmg
                UpdateLighthouseDmg(upgradeValue);
                break;
            case 3: //lh range
                UpdateLighthouseRange(upgradeValue);
                Debug.Log($"LighthouseRange is now {LighthouseManager.Instance.lighthouseRange}");
                break;  
            case 4: // player hp
                UpdatePlayerHealth(upgradeValue);
                break;
            case 5: // player mana
                UpdatePlayerMana(upgradeValue);
                break;
        }
    }

    private static void UpdateLighthouseHp(int bonusHp)
    {
        LighthouseManager.Instance.SetHp(50 + bonusHp * 5);
    }

    private static void UpdateLighthouseDmg(int bonusDmg)
    {
        LighthouseManager.Instance.SetDmg(bonusDmg);
    }

    private static void UpdateLighthouseRange(int bonusRange)
    {
        LighthouseManager.Instance.SetRadiusAndHeight(25 + bonusRange * 5, bonusRange);
    }

    private static void UpdatePlayerHealth(int bonusHp)
    {
        PlayerHealth.Instance.SetHealth(100 + 10 * bonusHp);
    }

    private static void UpdatePlayerMana(int bonusMana)
    {
        PlayerMana.Instance.SetMana(25 + bonusMana);
    }

    internal void UpdateAttributeLevelText(TextMeshProUGUI attributeLevelText, int attributeLevel)
    {
        attributeLevelText.text = $"Lvl: {attributeLevel.ToString()}";
    }

    internal void ResetUpgradeDictionary()
    {
        for (int i = 0; i < dictLength; i++)
        {
            upgradesDictionary[(UpgradeType)i] = 0;
        }
    }
}