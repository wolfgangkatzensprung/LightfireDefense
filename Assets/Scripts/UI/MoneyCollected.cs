using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyCollected : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        PlayerMoney.Instance.onGetMoney += PlayMoneyCollectedAnim;
    }

    void PlayMoneyCollectedAnim()
    {
        anim.Play("MoneyCollected");
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        return;
#endif
        PlayerMoney.Instance.onGetMoney -= PlayMoneyCollectedAnim;
    }
}
