using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructionTimer : MonoBehaviour
{
    [Tooltip("Time to self destruct in seconds")]
    public float selfDestructionTime = 10f;

    [Tooltip("Particles spawned on self destruct")]
    public GameObject particlesPrefab;

    void Start()
    {
        StartCoroutine(SelfDestruction());
    }

    IEnumerator SelfDestruction()
    {
        yield return new WaitForSeconds(selfDestructionTime);
        if (particlesPrefab != null)
            Instantiate(particlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
