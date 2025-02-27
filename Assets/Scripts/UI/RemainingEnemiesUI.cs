using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RemainingEnemiesUI : MonoBehaviour
{
    [Tooltip("Text of remaining enemies")]
    public TextMeshProUGUI remainingText;

    EnemyWaveSpawner ewsInstance;

    private void Start()
    {
        ewsInstance = EnemyWaveSpawner.Instance;
        ewsInstance.onMobSpawn += UpdateRemainingEnemiesUI;
        ewsInstance.onWaveStart += UpdateRemainingEnemiesUI;
        EnemyHandler.Instance.onMobDied += UpdateRemainingEnemiesUI;
    }

    internal void UpdateRemainingEnemiesUI()
    {
        remainingText.text = $"{EnemyHandler.Instance.GetRemainingMobsAmount()} remaining";
    }

}
