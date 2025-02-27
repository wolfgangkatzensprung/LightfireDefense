using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;

public class StartMenu : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("StartMenu Start()");
        GlobalInfo.Instance.mouseLook.SetCursorLock(false);
    }
}