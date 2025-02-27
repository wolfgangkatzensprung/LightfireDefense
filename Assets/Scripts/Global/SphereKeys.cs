using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SphereKeys
{
    public enum KeyType
    {
        Fire,
        Water,
        Earth,
        Air
    }
    static int keyArrayLength = 4;  // muss der Laenge des KeyType enums entsprechen

    public static int[] keyArray = new int[keyArrayLength];
    // 0 = Player besitzt den entsprechenden Key nicht
    // 1 = Player besitzt ihn

    internal static void AddKey(int keyType)
    {
        keyArray[keyType] = 1;
    }

    internal static int[] GetKeys()
    {
        return keyArray;
    }

    internal static bool HasKey(KeyType keyType)
    {
        if (keyArray[(int)keyType] == 0)
            return false;
        else return true;
    }

    internal static void LoadKeys(int[] keys)
    {
        Debug.Log("Load Keys");
        for (int i = 0; i < keys.Length; i++)
        {
            keyArray[i] = keys[i];
        }
    }

    internal static void ResetAllKeys()
    {
        keyArray = new int[keyArrayLength];
    }
}