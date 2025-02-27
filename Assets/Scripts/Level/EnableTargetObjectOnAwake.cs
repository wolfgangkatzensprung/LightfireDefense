using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTargetObjectOnAwake : MonoBehaviour
{
    public GameObject targetObject;

    private void Awake()
    {
        targetObject.SetActive(true);
    }
}
