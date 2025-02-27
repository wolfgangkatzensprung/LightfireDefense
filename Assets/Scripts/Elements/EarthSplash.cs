using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthSplash : MonoBehaviour
{
    private void Start()
    {
        if (transform.IsChildOf(GlobalInfo.Instance.playerTrans))
        {
            PlayerHealth.Instance.ApplyHeal(PlayerExp.Instance.level[(int)Damage.DamageType.Earth]);
        }
    }
}
