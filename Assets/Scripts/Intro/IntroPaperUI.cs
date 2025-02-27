using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPaperUI : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0f;
        GameController.StopGameLogic();
        UIManager.Instance.ShowMouse();
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            UIManager.Instance.HideIntroPaper();
        }
    }
    private void OnDisable()
    {
        UIManager.Instance.HideMouse();
        Time.timeScale = 1f;
        GameController.StartGameLogic();
    }
}
