using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public float yThreshold = -30f;
    private void Update()
    {
        if (GlobalInfo.Instance?.playerTrans.position.y < yThreshold)
        {
            PlayerHealth.Instance.ApplyDamage(100, Damage.DamageType.None);
        }
    }
}