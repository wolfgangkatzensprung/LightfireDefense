using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretManager : Singleton<TurretManager>
{
    public GameObject turretPrefab;

    /// <summary>
    /// Load Turrets from PlayerPrefs and spawn them
    /// </summary>
    public void LoadTurrets()
    {
        int amount = PlayerPrefs.GetInt("TurretAmount");

        for (int i = 0; i < amount; i++)
        {
            if (!PlayerPrefs.HasKey($"Turret{i}x"))
                continue;

            Vector3 pos = new Vector3(PlayerPrefs.GetFloat($"Turret{i}x"), PlayerPrefs.GetFloat($"Turret{i}y"), PlayerPrefs.GetFloat($"Turret{i}z"));

            GameObject turret = Instantiate(turretPrefab, pos, Quaternion.identity);

            turret.GetComponent<Turret>().turretNumber = i + 1;
            GameController.MoveObjectToCurrentLevelScene(turret);
        }
    }
    internal static void DeleteTurretObjects()
    {
        Turret[] turrets = GameObject.FindObjectsOfType<Turret>();
        foreach (Turret tur in turrets)
        {
            GameObject.Destroy(tur.gameObject);
        }
    }

    internal static void DeleteTurretPrefs()
    {
        if (PlayerPrefs.HasKey("TurretAmount"))
            PlayerPrefs.DeleteKey("TurretAmount");

        for (int i = 0; i < 15; i++)
        {
            if (PlayerPrefs.HasKey($"Turret{i}x"))
            {
                PlayerPrefs.DeleteKey($"Turret{i}x");
                PlayerPrefs.DeleteKey($"Turret{i}y");
                PlayerPrefs.DeleteKey($"Turret{i}z");
            }
        }

        if (GlobalInfo.Instance.playerTrans.gameObject.TryGetComponent(out PlayerBuild pb))
        {
            pb.ResetBuildingUI();
        }
    }
}
