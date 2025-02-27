using System;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class PlayerHealth : Singleton<PlayerHealth>, IDamageable
{
    public int maxHealth = 100;
    private int currentHealth = 100;
    internal int deltaHeal = 0;  // passive livereg
    int startDeltaHeal;
    float healRate = .3f;   // rate in seconds for passive healing
    float healTimer = 0f;   // timer for passive healing
    internal int orangeBuffLevel = 0;    // live reg bonus delta

    public delegate void DeathDelegate();
    public DeathDelegate onDeath;
    public delegate void OrangeBuffUpgradeDelegate();
    public OrangeBuffUpgradeDelegate onOrangeBuffUpgrade;

    public bool GetIsAlive()
    {
        return currentHealth > 0;
    }

    public void LoadHpAndOrangeBuff()   // Load from Savefile
    {
        SaveData data = SaveSystem.LoadGame();

        Debug.Log($"LoadHpFromSaveFile() mit data.upgrades[4] = {data.upgrades[4]}");

        maxHealth = 100 + 10 * data.upgrades[4];
        currentHealth = maxHealth;

        orangeBuffLevel = data.orangeBuff;
        deltaHeal = orangeBuffLevel;
    }

    private void Update()
    {
        TryUpdateHealthUI();

        if (orangeBuffLevel != 0 || SphereKeys.HasKey(SphereKeys.KeyType.Water))
            PassiveHealing();

        if (!GetIsAlive())
            Die();
    }

    private void PassiveHealing()
    {

        if (currentHealth < maxHealth)
        {
            healTimer += Time.deltaTime;
            if (healTimer > healRate)
            {
                ApplyHeal(deltaHeal);
                healTimer = 0f;
            }
        }
    }

    internal void UpgradeOrangeBuff()
    {
        orangeBuffLevel += 1;
        deltaHeal = SphereKeys.HasKey(SphereKeys.KeyType.Water) ? 1 : 0;
        deltaHeal = startDeltaHeal + orangeBuffLevel;

        SaveSystem.SaveGame();

        onOrangeBuffUpgrade?.Invoke();
    }
    internal void ResetOrangeBuff()
    {
        orangeBuffLevel = 0;
        deltaHeal = 0;
    }

    public void Die()
    {
        Debug.Log("Player died.");
        SetHealth(maxHealth);
        LighthouseManager.Instance.ApplyDamageToLighthouse(1);
        // lose exp?

        onDeath?.Invoke();

        SceneLoading.Instance.Respawn();
    }

    internal void SetHealth(int health)
    {
        maxHealth = health;
        currentHealth = Mathf.Max(currentHealth, maxHealth);
        TryUpdateHealthUI();
    }

    internal int GetHealth()
    {
        return currentHealth;
    }

    public void ApplyDamage(int damage, Damage.DamageType dmgType)
    {
        currentHealth = Mathf.Max(0, currentHealth - damage);
        SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.PlayerDamaged);
        PostProcessingEffects.DamagedEffect();
    }

    void TryUpdateHealthUI()
    {
        if (UIManager.Instance == null)
            return;

        string healthText = currentHealth.ToString();
        UIManager.Instance.SetHealthText(healthText);
    }

    public void ApplyDamage(int damage, Vector3 targetPosition, Vector3 hitPosition)
    {
        throw new NotImplementedException();
    }

    public void TryApplyDamage(int damage, Damage.DamageType damageType)
    {
        Debug.Log("Player got hit by own Damage. Nothing happens.");
    }

    internal void ApplyHeal(int healAmount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        UIManager.Instance.healedAnim.Play("Healed");
    }
}