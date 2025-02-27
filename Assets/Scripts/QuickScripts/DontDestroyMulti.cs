using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMulti : MonoBehaviour
{
    public GameObject[] dontDestroyObjects;

    void Start()
    {
        for (int i = 0; i < dontDestroyObjects.Length; i++)
        {
            DontDestroyOnLoad(dontDestroyObjects[i]);
        }
    }

}
