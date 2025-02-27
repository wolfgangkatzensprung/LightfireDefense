using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : ItemPickable, IInteractable
{
    [Tooltip("Prefab of Particles that spawn when Berry is eaten")]
    public GameObject eatBerryParticles;
    [Tooltip("Prefab of Blue Particles that spawn when Berry is finally enchanted")]
    public GameObject blueEnchantedParticles;
    [Tooltip("Prefab of Orange Particles that spawn when Berry is finally enchanted")]
    public GameObject orangeEnchantedParticles;

    public Material orangeParticleMaterial;
    public Material blueParticleMaterial;

    [Tooltip("AddForce Strength")]
    public float riseStrength = 5f;
    float riseSpeed = 1f;   // rising rise speed
    private LuckySprite luckySprite;    // dance partner for the rising dance
    float riseTimer = 0f;   // descending timer
    float riseTimerMax = 33f;    // seconds
    public float riseHeight = 66f;
    public LayerMask groundLayer;
    private bool rising;

    bool blueEnchantment;
    bool orangeEnchantment;

    public override void Unequip()
    {
        base.Unequip();
        GardenManager.Instance.AssignCurrentBerry(transform);
    }

    private void OnDestroy()
    {
#if UNITY_EDITOR
        return;
#endif
        GardenManager.Instance.RemoveBerry(gameObject);
    }

    private void FixedUpdate()
    {
        if (rising)
        {
            if (riseTimer > 0)
            {
                riseTimer -= Time.fixedDeltaTime;
                riseSpeed += Time.fixedDeltaTime;

                BerryDanceMovement();
            }
            else
            {
                FinishRisingDance();
            }
        }
    }

    private void BerryDanceMovement()
    {
        rb.MovePosition(transform.position + Vector3.up * riseSpeed * Time.fixedDeltaTime);
        //rb.AddForce(Vector3.up * riseStrength * Mathf.Min(Mathf.Max(0f, Mathf.Sin(Time.time)) * riseSpeed), riseSpeed) * Time.fixedDeltaTime);
        Debug.Log($"{gameObject.name} rising");
        if (transform.position.y > riseHeight)
        {
            FinishRisingDance();
        }
        else if (transform.position.y < 0)
        {
            RaycastHit groundHit;
            if (!Physics.Raycast(transform.position, Vector3.down, out groundHit, Mathf.Infinity, groundLayer))
            {
                if (Physics.Raycast(transform.position, Vector3.up, out groundHit, Mathf.Infinity, groundLayer))
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + groundHit.distance + 2f, transform.position.z);
                }
            }
        }
    }
    internal void StartRisingDance(LuckySprite ls)
    {
        luckySprite = ls;

        rb.velocity = Vector3.up * 3f;
        riseTimer = riseTimerMax;
        rising = true;

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        ps.emissionRate = 12f;  // start emissionRate is 3
        ps.gravityModifier = 0f;

        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    private void FinishRisingDance()
    {
        ParticleSystemRenderer psRend = GetComponentInChildren<ParticleSystemRenderer>();

        rising = false;
        rb.velocity = (Vector3.zero - transform.position).normalized * 10f + Vector3.up * 15f;
        riseTimer = 0f;

        if (luckySprite.isOrange)
        {
            Instantiate(orangeEnchantedParticles, transform.position, Quaternion.identity);
            psRend.sharedMaterial = orangeParticleMaterial;
            orangeEnchantment = true;
        }
        else
        {
            Instantiate(blueEnchantedParticles, transform.position, Quaternion.identity);
            psRend.sharedMaterial = blueParticleMaterial;
            blueEnchantment = true;
        }

        LuckySprite.FinishBerryDance();
        luckySprite.ResetModeToIdle();
        luckySprite.ResetConnectionLine();
        luckySprite.HappyRotation();

        gameObject.name = "Enchanted Berry";
        gameObject.layer = LayerMask.NameToLayer("Item");

        if (gameObject.TryGetComponent(out MeshRenderer rend))
        {
            Debug.Log($"Set Material for Enchanted Berry");
            rend.material = luckySprite.GetComponentInChildren<MeshRenderer>().material;
        }

        GardenManager.Instance.RemoveBerry(gameObject);

        Debug.Log($"{gameObject.name} successfully risen by dancing with {luckySprite.name}");
    }

    public void Interact()
    {
        if (blueEnchantment)
            PlayerMana.Instance.UpgradeBlueBuff();
        else if (orangeEnchantment)
            PlayerHealth.Instance.UpgradeOrangeBuff();

        PlayerHealth.Instance.ApplyHeal(25);
        if (GlobalInfo.Instance.heldItem != null && GlobalInfo.Instance.heldItem.Equals(gameObject))
            GlobalInfo.Instance.SetHeldItem(null);
        Instantiate(eatBerryParticles, GlobalInfo.Instance.itemHolder.position, Quaternion.identity);
        Destroy(gameObject);
    }
}