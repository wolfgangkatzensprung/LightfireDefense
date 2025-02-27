using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShadowBlurperMovement : MonoBehaviour
{
    Rigidbody rb;
    public float radius = 500f;
    [Tooltip("Horizontal Speed")]
    public float speed = .1f;
    [Tooltip("Vertical Speed")]
    public float ySpeed = 1f;

    [Tooltip("Maximum and minimum value for y position")]
    public float maxAmplitude = 100f;

    Vector3 targetPos = new Vector3();

    public float zRotationVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        targetPos = GetTargetPosition();
        Vector3 targetVelocity = (targetPos - transform.position) * speed;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref targetVelocity, Time.deltaTime);

        //Debug.Log($"targetPos: {targetPos}");
        //rb.velocity = targetVelocity;
    }
    void FixedUpdate()
    {
        Quaternion dirRotation = Quaternion.FromToRotation(transform.position, targetPos);
        Quaternion deltaRotation = Quaternion.Euler(targetPos * Time.fixedDeltaTime);
        rb.MoveRotation(dirRotation * deltaRotation);
    }

    private Vector3 GetTargetPosition()
    {
        float x = Mathf.Cos(Time.time * speed) * radius;
        float z = Mathf.Sin(Time.time * speed) * radius;

        float sineY = Mathf.Sin(Time.time * ySpeed) * maxAmplitude;
        float pingPong = (Mathf.PingPong(Time.time * .3f, 1f) + 1) * .5f;
        float perlin = Mathf.PerlinNoise(transform.position.x + pingPong, transform.position.z + pingPong);

        float y = sineY * perlin;

        return new Vector3(x, y, z);
    }
}