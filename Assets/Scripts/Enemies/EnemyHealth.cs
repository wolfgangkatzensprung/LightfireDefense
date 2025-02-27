using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [Tooltip("Mob Health is = maxHp + waveNumber*2 + 1")]
    public int maxHp = 10;
    internal int currentHealth { get; private set; }
    private bool dead;

    [Tooltip("Typ des Mob. Er ist resistent gegen diesen Damage Type und kriegt doppelten Schaden vom Konterelement")]
    public Damage.DamageType mobType;

    [Tooltip("True if this is a tower defense mob that has Enemy_TD Component")]
    public bool tdMob;

    public delegate void DamagedDelegate(Damage.DamageType dmgType);
    public DamagedDelegate onDamaged;

    public delegate void DeathDelegate();
    public DeathDelegate onDeath;

    private void Start()
    {
        if (GlobalInfo.Instance != null)
            currentHealth = maxHp + GlobalInfo.Instance.waveNumber * 2 + 1;
    }

    #region ApplyDamage Overloads

    public void ApplyDamage(int damage, Damage.DamageType dmgType)
    {
        Debug.Log($"{gameObject.name} takes {damage} damage of type {dmgType}");
        if (damage == 0)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.EnemyDamaged, transform.position);

        onDamaged?.Invoke(dmgType);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void TryApplyDamage(int damage, Damage.DamageType damageType)
    {
        Debug.Log($"Applying {damage} {damageType.ToString()} Damage to {gameObject.name}.");

        if ((int)mobType != 0)
        {

            if (damageType == mobType)    // Resistenz gegen gleiches Element
            {
                ApplyDamage(damage / 2, damageType);
                Debug.Log($"Damage was reduced to {damage / 2} due to {mobType.ToString()} resistance.");
            }
            else
            {
                ApplyElementalDamage(damageType, mobType, damage);  // Double Damage gegen Konter-Element
            }
        }
        else
        {
            ApplyDamage(damage, damageType);
        }
    }

    private void ApplyElementalDamage(Damage.DamageType damageType, Damage.DamageType mobType, int rawDamage)
    {
        int finalDamage = rawDamage;
        switch (mobType)
        {
            case Damage.DamageType.Air:
                if (damageType.Equals(Damage.DamageType.Earth))
                    finalDamage = rawDamage * 2;
                break;
            case Damage.DamageType.Earth:
                if (damageType.Equals(Damage.DamageType.Air))
                    finalDamage = rawDamage * 2;
                break;
            case Damage.DamageType.Fire:
                if (damageType.Equals(Damage.DamageType.Water))
                    finalDamage = rawDamage * 2;
                break;
            case Damage.DamageType.Water:
                if (damageType.Equals(Damage.DamageType.Fire))
                    finalDamage = rawDamage * 2;
                break;
        }

        ApplyDamage(finalDamage, damageType);
    }

    #endregion ApplyDamage Overloads

    public bool GetIsAlive()
    {
        return currentHealth > 0;
    }

    public void Die()
    {
        if (dead)
            return;

        dead = true;

        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.EnemyDeath, transform.position);

        PlayerShooting.Instance.IncreaseKillCount(1);

        if (tdMob)
            EnemyHandler.Instance.RemoveEnemy(gameObject);

        onDeath?.Invoke();
        Destroy(gameObject);
    }
}