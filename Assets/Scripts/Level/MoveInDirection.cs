using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDirection : MonoBehaviour
{
    [Tooltip("Speed and Direction")]
    public Vector3 speed = Vector3.forward;

    private void Update()
    {
        transform.Translate(speed);
    }
}
