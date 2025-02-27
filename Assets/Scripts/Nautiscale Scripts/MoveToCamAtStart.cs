using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToCamAtStart : MonoBehaviour
{
    public Vector3 offset;
    void Start()
    {
        transform.position = Camera.main.transform.position + offset;
    }
}
