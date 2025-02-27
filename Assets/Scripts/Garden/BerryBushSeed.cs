using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BerryBushSeed : ItemPickable
{
    bool canPlant;

    public override void Unequip()
    {
        base.Unequip();
        canPlant = true;
        Debug.Log("CAN PLANT SEED");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canPlant && collision.collider.CompareTag("Ground") && Vector3.Distance(transform.position, Vector3.zero) < LighthouseManager.Instance.lighthouseRange)
        {
            PlantSeed(collision.GetContact(0).point);
            Destroy(gameObject);
        }
    }

    private void PlantSeed(Vector3 pos)
    {
        Debug.Log("Plant Seed");
        GameObject berryBush = Instantiate(GardenManager.Instance.berryBushPrefab, pos, Quaternion.identity);
        berryBush.transform.Rotate(Vector3.up, Random.Range(0f, 360f));
    }
}
