using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomName : MonoBehaviour
{
    public string customName = "";
    private void Awake()
    {
        if (customName != "")
            gameObject.name = customName;
    }
}