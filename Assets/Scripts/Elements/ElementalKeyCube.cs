using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementalKeyCube : MonoBehaviour
{
    public SphereKeys.KeyType keyType;

    float movementTimer = 0f;
    public float movementTime = 25f;    // time in seconds until direction flip
    float movementDirection = 1;
    Vector3 dir;

    bool cubeActive;

    private void OnEnable()
    {
        int[] keys = SphereKeys.GetKeys();


        if (keys[(int)keyType] > 0)
        {
            cubeActive = true;
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (cubeActive)
        {
            foreach (Transform child in transform)
            {
                dir = child.position - transform.position;
                Debug.Log("Dir " + dir);
                SphereMovement(child);
            }
        }
    }

    private void SphereMovement(Transform child)
    {
        movementTimer += Time.deltaTime;
        if (movementTimer > movementTime)
        {
            movementDirection *= -1f;
            movementTimer = 0f;
        }
        child.localPosition += dir.normalized * Time.deltaTime * movementDirection;
    }

    private void PingPongScale(Transform child)
    {
        Vector3 playerPos = GlobalInfo.Instance.playerTrans.position;
        float perlin = Mathf.PerlinNoise(playerPos.x, playerPos.y);
        float pingPong = (.1f + Mathf.PingPong(Time.time, .9f));
        Vector3 scale = Vector3.Lerp(child.localScale, Vector3.one, pingPong);
        Debug.Log("Elemental PingPong:" + pingPong);
        Debug.Log("Elemental Scale:" + scale);
        child.localScale = pingPong * Vector3.one;
    }
}
