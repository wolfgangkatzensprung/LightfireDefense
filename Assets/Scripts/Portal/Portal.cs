using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public string targetLocation = "FluxCube";
    public Transform targetPosition;

    public void UsePortal()
    {
        SaveSystem.SaveGame();
        Debug.Log($"UsePortal() {targetLocation}");

        if (targetPosition != null)
            GlobalInfo.Instance.playerTrans.position = targetPosition.position;

        if (targetLocation == null)
        {
            targetLocation = "TD Level";
        }

        SceneLoading.Instance.LoadLevel(targetLocation);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GlobalInfo.Instance.playerRb.velocity = Vector3.zero;
            UsePortal();
        }
    }
}