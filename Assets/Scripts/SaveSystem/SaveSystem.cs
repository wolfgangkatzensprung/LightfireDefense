using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using System;

public static class SaveSystem
{
    static string saveFile = "/LighthouseSaveFile.sav";

    public delegate void SavingGameDelegate();
    public static SavingGameDelegate onStartSavingGame;

    public static void SaveGame()
    {
        onStartSavingGame?.Invoke();

        SaveData data = new SaveData
            (
            PlayerExp.Instance.exp,
            PlayerExp.Instance.level,
            PlayerMoney.Instance.money,
            EnemyWaveSpawner.Instance.GetWaveIndex(),
            PlayerShooting.Instance.killCount,
            UpgradeManager.Instance.GetUpgrades(),
            LighthouseManager.Instance.currentLighthouseHp,
            SphereKeys.GetKeys(),
            PlayerHealth.Instance.orangeBuffLevel,
            PlayerMana.Instance.blueBuffLevel
            ) ;

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + saveFile;

        FileStream stream = new FileStream(path, FileMode.Create);
        try
        {
            formatter.Serialize(stream, data);
        }
        catch (SerializationException e)
        {
            Debug.Log($"Serialization Failed. Reason: {e.Message}");
            throw;
        }
        finally
        {
            for (int i = 0; i < data.upgrades.Length; i++)
            {
                Debug.Log($"Upgrade {i}: {data.upgrades[i]}");
            }
            stream.Close();
        }
    }

    internal static bool GetSaveFileExists()
    {
        string path = Application.persistentDataPath + saveFile;
        if (File.Exists(path))
        {
            return true;
        }
        else return false;
    }

    public static SaveData LoadGame()
    {
        Debug.Log("LoadGame()");
        string path = Application.persistentDataPath + saveFile;
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSaveGame()
    {
        TrapManager.DeleteTrapPrefs();
        TrapManager.DeleteTrapObjects();

        TurretManager.DeleteTurretPrefs();
        TurretManager.DeleteTurretObjects();

        GardenManager.DeleteBerryBushPrefs();
        GardenManager.DeleteBerryBushObjects();

        ClearPortalPrefs();

        string path = Application.persistentDataPath + saveFile;
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("saveFile deleted.");
        }
    }

    private static void ClearPortalPrefs()
    {
        if (PlayerPrefs.HasKey("PortalReady"))
            PlayerPrefs.DeleteKey("PortalReady");
        if (PlayerPrefs.HasKey("PortalPieces"))
            PlayerPrefs.DeleteKey("PortalPieces");
    }
}