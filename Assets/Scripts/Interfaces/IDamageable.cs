using UnityEngine;

public interface IDamageable
{
    bool GetIsAlive();
    void ApplyDamage(int damage, Damage.DamageType dmgType);

    void TryApplyDamage(int damage, Damage.DamageType damageType);

    void Die();
}