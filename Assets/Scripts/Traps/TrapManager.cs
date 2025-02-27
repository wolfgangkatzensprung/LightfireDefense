using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapManager : Singleton<TrapManager>
{
    public GameObject[] trapPrefabs;
    internal int trapAmount = 0;    // actual current trap amount on map

    private void Start()
    {
        SceneLoading.Instance.onSceneLoadedAsync += ResetTrapAmount;
    }

    private void ResetTrapAmount(string sceneName)
    {
        trapAmount = 0;
    }

    public void TryAddTrap(Trap trap)
    {
        trap.trapIndex = trapAmount;
        trapAmount += 1;
        PlayerPrefs.SetInt("TrapAmount", trapAmount);   // total trap amount of traps that ever existed
        SaveTrap(trap);
    }

    public void SaveTrap(Trap trap)
    {
        int i = trap.trapIndex;
        PlayerPrefs.SetFloat($"Trap{i}x", trap.transform.position.x);
        PlayerPrefs.SetFloat($"Trap{i}y", trap.transform.position.y);
        PlayerPrefs.SetFloat($"Trap{i}z", trap.transform.position.z);
        PlayerPrefs.SetInt($"Trap{i}type", (int)trap.trapType);
        PlayerPrefs.SetInt($"Trap{i}exists", 1);

        Debug.Log("Trap" + i + " Position and Type saved.");
    }

    public void UnsaveTrap(Trap trap)
    {
        int i = trap.trapIndex;
        PlayerPrefs.SetInt($"Trap{i}exists", 0);
    }

    /// <summary>
    /// Load Traps from PlayerPrefs and spawn them
    /// </summary>
    internal void LoadTraps()
    {
        Debug.Log("LoadTraps");
        int amount = PlayerPrefs.GetInt("TrapAmount");

        for (int i = 0; i < amount; i++)
        {
            if (!PlayerPrefs.HasKey($"Trap{i}x") || PlayerPrefs.GetInt($"Trap{i}exists") < 1)
                continue;

            Vector3 pos = new Vector3(PlayerPrefs.GetFloat($"Trap{i}x"), PlayerPrefs.GetFloat($"Trap{i}y"), PlayerPrefs.GetFloat($"Trap{i}z"));
            int trapType = PlayerPrefs.GetInt($"Trap{i}type");

            GameObject trap = Instantiate(trapPrefabs[trapType], pos, Quaternion.identity);
            trap.GetComponent<Trap>().trapIndex = i;

            GameController.MoveObjectToCurrentLevelScene(trap);
            Debug.Log($"Trap{i} loaded");
        }
    }
    internal static void DeleteTrapObjects()
    {
        Trap[] traps = GameObject.FindObjectsOfType<Trap>();
        foreach (Trap tr in traps)
        {
            GameObject.Destroy(tr.gameObject);
        }
    }

    internal static void DeleteTrapPrefs()
    {
        if (PlayerPrefs.HasKey("TrapAmount"))
            PlayerPrefs.DeleteKey("TrapAmount");

        int maxTrapAmount = 999;

        for (int i = 0; i < maxTrapAmount; i++)
        {
            if (PlayerPrefs.HasKey($"Trap{i}x"))
            {
                PlayerPrefs.DeleteKey($"Trap{i}x");
                PlayerPrefs.DeleteKey($"Trap{i}y");
                PlayerPrefs.DeleteKey($"Trap{i}z");
                PlayerPrefs.DeleteKey($"Trap{i}exists");
            }
            else continue;
        }
    }
}