using ECM.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInfo : Singleton<GlobalInfo>
{
    [Header("VERSION")]
    public string version;

    [Header("Player References")]
    [Tooltip("Transform of Player")]
    public Transform playerTrans;

    [Tooltip("Transform of Main Camera")]
    public Transform mainCam;

    [Tooltip("Rigidbody of Player")]
    public Rigidbody playerRb;

    [Tooltip("Magic Wand in Player's hands")]
    public GameObject magicWand;

    [Tooltip("Fire Position child on Player")]
    public Transform firePoint;

    [Tooltip("Item Holder child on Player")]
    public Transform itemHolder;
    [Tooltip("Item in der Hand")]
    public GameObject heldItem;

    [Tooltip("MouseLook Component")]
    public MouseLook mouseLook;

    [Header("Other References")]
    [Tooltip("Prefab of turret for spawning it in")]
    public GameObject turretPrefab;
    [Tooltip("Transform of the orange LuckySprite")]
    public Transform luckySpriteTrans;

    [Tooltip("Enemy Layer")]
    public LayerMask enemyLayer;
    [Tooltip("Item Layer for button Q")]
    public LayerMask itemLayer;
    [Tooltip("Interactable Layer for button E")]
    public LayerMask interactableLayer;
    [Tooltip("Interactive Layer for spellcasts etc")]
    public LayerMask interactiveLayer;

    internal static bool isNewStart { get; set; }  // if game is started from New Game Button
    internal static bool inMenu { get; set; }  // more accessible version of UIManager.Instance.inMenu
    internal static bool inIntro { get; set; } // more accessible version of UIManager.Instance.inIntro

    public int waveNumber { get; set; }

    public override void Awaken()
    {
        inMenu = true;
    }

    internal void SetHeldItem(GameObject heldItem)
    {
        this.heldItem = heldItem;
    }
    internal GameObject GetHeldItem()
    {
        return heldItem;
    }

    internal float GetDistanceToPlayer(Vector3 position)
    {
        return Vector3.Distance(position, playerTrans.position);
    }
}