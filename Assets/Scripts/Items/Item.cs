using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string customName = "";

    private void Start()
    {
        ItemHandler.Instance.AddItem(gameObject);
        if (customName != "")
        {
            gameObject.name = customName;
        }
    }
    private void OnDestroy()
    {
        if (ItemHandler.Instance != null)
            ItemHandler.Instance.RemoveItem(gameObject);
    }
}