using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMoneyButton : MonoBehaviour
{
    public void GetDemMoneys()
    {
        PlayerMoney.Instance.AddMoney(99999);
    }
}