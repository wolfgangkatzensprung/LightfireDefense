using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FloatingMotion : MonoBehaviour
{
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (GlobalInfo.inMenu)
            return;

        Vector3 translation = Vector3.up * Mathf.Sin(Time.time) * Time.deltaTime * .1f;
        transform.Translate(translation, Space.World);
    }
}
