using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCursorOnStart : MonoBehaviour
{
    private void Start()
    {
        LockCursor();
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        LockCursor();
    }
}
