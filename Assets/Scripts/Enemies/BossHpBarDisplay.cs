using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BossHpBarDisplay : MonoBehaviour
{
    EnemyHealth bossEh;
    Image healthBar;

    bool active;

    private void Start()
    {
        healthBar = GetComponent<Image>();
    }

    private void Update()
    {
        if (active)
            DisplayHealth();
    }

    internal void EnableBossHpBar(EnemyHealth bossEh)
    {
        active = true;
        this.bossEh = bossEh;
        bossEh.onDeath += DisableBossHpBarDisplay;
    }

    private void DisplayHealth()
    {
        healthBar.fillAmount = (float)bossEh.currentHealth / (float)bossEh.maxHp;
        Debug.Log($"Boss Hp: {bossEh.currentHealth}, Fill Amount: {healthBar.fillAmount}");
    }

    private void DisableBossHpBarDisplay()
    {
        active = false;
        UIManager.Instance.HideBossHpBar();
        bossEh.onDeath -= DisableBossHpBarDisplay;
    }

}
