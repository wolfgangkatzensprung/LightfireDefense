using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    [Tooltip("UpgradeType Attribute")]
    public UpgradeManager.UpgradeType upgradeAttribute;

    TextMeshProUGUI attributeLevelText;
    TextMeshProUGUI upgradeButtonText;

    int[] upgradeCosts =
    {
        100,
        250,
        500,
        750,
        1000,
        1500,
        2000,
        3000,
        4000,
        5000,
        6000,
        7000,
        8000,
        9000,
        10000,
        15000,
        20000,
        25000,
        30000,
        35000,
        40000,
        45000,
        50000,
        65000,
        70000,
        75000,
        80000,
        85000,
        90000,
        100000
    };
    [Tooltip("Current update level of this attribute")]
    int currentUpgradeLevel = 0;

    void LoadButton()
    {
        int upgrade = UpgradeManager.Instance.GetUpgrades()[(int)upgradeAttribute];
        currentUpgradeLevel = upgrade;
        //Debug.Log($"Load UpgradeButton {gameObject.name} Upgrade Level: {upgrade}. Index: {(int)upgradeAttribute}");
        SetUpgradeButtonTextUI();
        UpgradeManager.Instance.UpdateAttributeLevelText(attributeLevelText, upgrade);
    }

    public void UpgradeAttribute()  // Button Click
    {
        if (currentUpgradeLevel == upgradeCosts.Length)
        {
            Debug.Log("UpgradeAttribute is already max.");
            return;
        }

        if (PlayerMoney.Instance.money >= upgradeCosts[currentUpgradeLevel])
        {
            Debug.Log($"Upgrade {upgradeAttribute} UpgradeLevel from {currentUpgradeLevel} to {currentUpgradeLevel + 1}");
            DoUpgrade();
            SetUpgradeButtonTextUI();
        }
        else
        {
            Debug.Log($"Not enough money to upgrade {upgradeAttribute.ToString()} Attribute.");
            UIManager.Instance.PlayNotEnoughMoneyAnim();
            SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.Error);
        }
    }

    private void DoUpgrade()
    {
        PlayerMoney.Instance.RemoveMoney(upgradeCosts[currentUpgradeLevel]);
        currentUpgradeLevel += 1;
        UpgradeManager.Instance.Upgrade(upgradeAttribute, attributeLevelText);
    }
    
    private void SetUpgradeButtonTextUI()
    {
        if (currentUpgradeLevel == upgradeCosts.Length - 1)
        {
            upgradeButtonText.text = "Max";
        }
        else if (currentUpgradeLevel < upgradeCosts.Length - 1)
        {
            float nextUpgradeCost = upgradeCosts[currentUpgradeLevel];
            upgradeButtonText.text = nextUpgradeCost.ToString();
        }
        attributeLevelText.text = $"Level: {currentUpgradeLevel.ToString()}";
    }

    private void OnEnable()
    {
        attributeLevelText = transform.Find("LevelText").GetComponent<TextMeshProUGUI>();
        upgradeButtonText = transform.Find("UpgradeButtonText").GetComponent<TextMeshProUGUI>();

        LoadButton();
    }
}