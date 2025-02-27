using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_MinHeight : MonoBehaviour
{
    [Tooltip("GameObject will be disabled if Screen Height is lower than this value")]
    public float minHeight = 1028;

    private void Awake()
    {
        Debug.Log($"Screen Width: {Screen.width}");
        if (Screen.height < minHeight)
            gameObject.SetActive(false);
    }
}
