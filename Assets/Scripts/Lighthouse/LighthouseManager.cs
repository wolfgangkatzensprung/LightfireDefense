using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseManager : Singleton<LighthouseManager>
{
    [Tooltip("How many enemies may reach lighthouse")]
    public int maxLighthouseHp = 50;

    [Tooltip("How many lives are left")]
    public int currentLighthouseHp = 50;

    [Tooltip("Damage dealt by Lighthouse Laser")]
    public int lighthouseDmg = 0;

    [Tooltip("Lighthouse Light Radius (Default 25) plus Bonus Range based on Lighthouse Upgrade Lvl")]
    public int lighthouseRange = 25;
    internal int lighthouseHeight = 0;

    public delegate void RadiusChangeDelegate();
    public RadiusChangeDelegate onRadiusChange;

    private void Start()
    {
        currentLighthouseHp = maxLighthouseHp;
    }

    internal void Initialize()  // called from UI Manager
    {
        UpgradeManager.Instance.onUpgrade += UpgradeTower;
    }

    void UpgradeTower(UpgradeManager.UpgradeType attribute)
    {
        switch (attribute)
        {
            case UpgradeManager.UpgradeType.LighthouseHp:
                maxLighthouseHp += 1;
                break;
            case UpgradeManager.UpgradeType.LighthouseDmg:
                lighthouseDmg += 1;
                break;
            case UpgradeManager.UpgradeType.LighthouseRange:
                lighthouseRange += 1;
                break;
        }
    }

    internal void SetDefaults()
    {
        SetHp(50);
        SetCurrentLives(50);
        SetDmg(0);
        SetRadiusAndHeight(25, 0);
    }

    internal void SetCurrentLives(int towerHp)
    {
        currentLighthouseHp = towerHp;
    }

    internal void SetHp(int hp)
    {
        maxLighthouseHp = hp;
        currentLighthouseHp = Mathf.Max(currentLighthouseHp, maxLighthouseHp);
        UIManager.Instance.UpdateLighthouseHp();
    }

    internal void SetDmg(int dmg)
    {
        lighthouseDmg = dmg;
    }

    internal void SetRadiusAndHeight(int radius, int height)
    {
        lighthouseRange = radius;
        lighthouseHeight = height;
        onRadiusChange?.Invoke();

    }

    public void ApplyDamageToLighthouse(int dmg)
    {
        currentLighthouseHp = Mathf.Max(currentLighthouseHp - dmg, 0);
        UIManager.Instance.UpdateLighthouseHp();

        if (currentLighthouseHp <= 0)
        {
            GameController.Instance.LoseGame();
        }
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        if (UpgradeManager.Instance != null)
            UpgradeManager.Instance.onUpgrade -= UpgradeTower;
    }
}
