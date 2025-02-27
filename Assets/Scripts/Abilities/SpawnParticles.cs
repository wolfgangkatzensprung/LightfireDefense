using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnParticles : MonoBehaviour
{
    public GameObject particlesPrefab;
    public Vector3 offset;

    public void SpawnTheParticles()
    {
        Instantiate(particlesPrefab, transform.position + offset, Quaternion.identity);
    }
}
