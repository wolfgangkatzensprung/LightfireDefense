using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleIncreaser : MonoBehaviour
{
    public float scaleIncreaseSpeed = 1f;
    void Update()
    {
        transform.localScale += Vector3.one * Time.deltaTime * scaleIncreaseSpeed;
    }
}
