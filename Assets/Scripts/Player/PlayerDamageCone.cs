using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageCone : MonoBehaviour
{
    public Damage.DamageType dmgType = Damage.DamageType.Fire;

    float timer = 0f;
    public float delayBetweenTicks = 0.1f;
    bool rdyForNextTick;

    private void OnTriggerStay(Collider other)
    {
        timer += Time.deltaTime;
        if (timer > delayBetweenTicks)
        {
            rdyForNextTick = true;
            timer = 0;
        }

        if (rdyForNextTick && other.TryGetComponent(out IDamageable eh))
        {
            rdyForNextTick = false;
            eh.TryApplyDamage(PlayerExp.Instance.level[(int)dmgType] + PlayerShooting.Instance.currentBonusDmg, dmgType);
            PlayerMana.Instance.UseMana_Attack();
        }
    }

    private void OnEnable()
    {
        timer = 0;
    }
}