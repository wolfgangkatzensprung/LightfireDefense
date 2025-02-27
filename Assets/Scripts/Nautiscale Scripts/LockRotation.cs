using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    int rotationX;

    void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(rotationX, 0, 0);
    }
}