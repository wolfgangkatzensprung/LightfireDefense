using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLootManager : Singleton<RandomLootManager>
{
    [Tooltip("One of these will be spawned randomly where an enemy should drop an item")]
    public GameObject[] commonItems = new GameObject[1];
    public GameObject[] rareItems = new GameObject[1];

    public void SpawnRandomItem(Vector3 enemyPosition)
    {
        int randomRarity = Random.Range(0, 2);
        int randomResult;

        if (randomRarity == 0)
        {
            Debug.Log("Drop Common Item");
            randomResult = Random.Range(0, commonItems.Length);
            Instantiate(commonItems[randomResult], enemyPosition, Quaternion.identity);
        }
        else if (randomRarity == 1)
        {
            Debug.Log("Drop Rare Item");
            randomResult = Random.Range(0, rareItems.Length);
            Instantiate(rareItems[randomResult], enemyPosition, Quaternion.identity);
        }
    }
}