using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private void Start()
    {
        if (GlobalInfo.Instance != null && GlobalInfo.Instance.playerTrans != null && !GlobalInfo.isNewStart)
        {
            Debug.Log("Player SpawnPoint loaded");
            //GlobalInfo.Instance.playerTrans.position = transform.position;
            //GlobalInfo.Instance.playerTrans.rotation = transform.rotation;
            GameController.Instance.SetPlayerPositionAndRotation(transform.position, transform.rotation);
        }
    }
}