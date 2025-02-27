using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransformPosition : MonoBehaviour
{
    [Tooltip("Target Transform. Null = Player")]
    public Transform targetTrans;
    public Transform targetPos;


    private void Start()
    {
        if (targetTrans != null)
            return;

        targetTrans = GlobalInfo.Instance.playerTrans;
    }
    public void SetPosition()
    {
        targetTrans.position = targetPos.position;
    }
}