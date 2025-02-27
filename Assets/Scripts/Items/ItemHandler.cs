using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : Singleton<ItemHandler>
{
    List<GameObject> itemsOnMap = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        itemsOnMap.Add(item);
    }

    public void RemoveItem(GameObject item)
    {
        itemsOnMap.Remove(item);
    }

    public List<GameObject> GetItemsOnMap()
    {
        return itemsOnMap;
    }

    public void ClearItems()
    {
        Debug.Log($"Clearing all {itemsOnMap.Count} Items");

        GameObject[] itemsArray = itemsOnMap.ToArray();

        for (int i = 0; i < itemsArray.Length; i++)
        {
            Destroy(itemsArray[i]);
        }

        Debug.Log($"Items cleared. {itemsOnMap.Count} remaining.");
    }
}