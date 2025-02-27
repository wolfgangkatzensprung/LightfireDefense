using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTransform : MonoBehaviour
{
    public float rotationSpeed = 2.3f;

    public Vector3 rotationAxis = Vector3.forward;

    private void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed);
    }
}