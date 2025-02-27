using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwim : MonoBehaviour
{
    private void Update()
    {

        return;

        if (GlobalInfo.Instance.playerTrans.position.y < 0)
        {
            Debug.Log("Swimmedy Swim");
        }
    }
}