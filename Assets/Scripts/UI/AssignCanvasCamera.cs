using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignCanvasCamera : MonoBehaviour
{
    Canvas c;
    private void Start()
    {
        if (GlobalInfo.Instance == null)
        {
            enabled = false;
            return;
        }
        c = GetComponent<Canvas>();
        c.worldCamera = GlobalInfo.Instance.mainCam.GetComponent<Camera>();
    }
}
