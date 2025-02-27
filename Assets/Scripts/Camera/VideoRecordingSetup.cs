using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoRecordingSetup : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
    }
    private void Update()
    {
        if (GameController.Instance != null && Input.GetKeyDown(KeyCode.U))
        {
            GlobalInfo.Instance.playerTrans.gameObject.SetActive(false);
            UIManager.Instance.UI.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            Time.timeScale = 1f;
        }
    }
}