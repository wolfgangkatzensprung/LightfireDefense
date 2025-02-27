using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{    
    [Tooltip("Maximale Lebenszeit in SEKUNDEN")]
    public float maxLifeTime = 15f;
    float lifeTimer = 0;
    private void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > maxLifeTime)
        {
            Destroy(gameObject);
        }
    }
}
