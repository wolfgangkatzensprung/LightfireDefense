using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpawnWaveInputField : MonoBehaviour
{
    EnemyWaveSpawner ews;
    TMP_InputField inputField;

    private void Start()
    {
        ews = GameObject.Find("ENEMIES").GetComponent<EnemyWaveSpawner>();
        inputField = GetComponent<TMP_InputField>();
    }

    public void TrySpawnWaveByUI()
    {
        if (int.TryParse(inputField.text, out int waveIndex))
        {
            if (waveIndex > 50 || waveIndex < 0)
            {
                return;
            }
            ews.SetWaveIndex(waveIndex);
        }
    }
}
