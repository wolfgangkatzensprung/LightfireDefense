using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsModule : MonoBehaviour
{
    public GameObject instaCamPrefab;

    Transform cheatsTrans;

    private void Start()
    {
        cheatsTrans = transform.GetChild(0).transform;
    }
    void Update()
    {
        if (GlobalInfo.inMenu && Input.GetKeyDown(KeyCode.C))
        {
            cheatsTrans.gameObject.SetActive(!cheatsTrans.gameObject.activeSelf);
        }
        else if (Input.GetButtonDown("Escape") || Input.GetButtonDown("Tab"))
            cheatsTrans.gameObject.SetActive(false);
        else if (Input.GetKeyDown(KeyCode.I))
        {
            StartFreeCamMode();
        }
    }

    private void StartFreeCamMode()
    {
        Instantiate(instaCamPrefab, GlobalInfo.Instance.playerTrans.position + Vector3.up * 3f, Quaternion.identity);
        GlobalInfo.Instance.playerTrans.gameObject.SetActive(false);
        UIManager.Instance.UI.SetActive(false);
        gameObject.SetActive(false);
    }
}