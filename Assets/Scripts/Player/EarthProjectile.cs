using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthProjectile : Projectile
{
    [Tooltip("Earth Splash VFX Prefab")]
    public GameObject earthSplash;

    private void Start()
    {
        SoundManager.Instance.PlaySoundAt(SoundManager.Sound.EarthProjectile, transform.position);
        PlayerInputManager.Instance.onItemPickup += OnPickup;

        StartCoroutine(BlinkRoutine());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent(out IDamageable eh))
        {
            eh.TryApplyDamage(PlayerShooting.Instance.damageMultiplier * PlayerExp.Instance.level[(int)damageType], damageType);

            Die();
        }
        else
        {
            gameObject.layer = 10;  // Item Layer
            gameObject.AddComponent<ItemPickable>();
        }
    }

    IEnumerator BlinkRoutine()
    {
        Material mat = GetComponent<Renderer>().material;
        float startEmissionIntensity = mat.GetFloat("_EmissiveIntensity");

        yield return new WaitForSeconds(selfDestructionTime - 3);
        mat.SetFloat("_EmissiveIntensity", 0f);
        mat.SetColor("_EmissiveColor", mat.color);
        yield return new WaitForSeconds(1);
        mat.SetFloat("_EmissiveIntensity", startEmissionIntensity);
        mat.SetColor("_EmissiveColor", mat.color);
        yield return new WaitForSeconds(1);
        mat.SetFloat("_EmissiveIntensity", 0f);
        mat.SetColor("_EmissiveColor", mat.color);
        yield return new WaitForSeconds(.3f);
        mat.SetFloat("_EmissiveIntensity", startEmissionIntensity);
        mat.SetColor("_EmissiveColor", mat.color);
    }

    private void Die()
    {
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject, selfDestructionTime * .5f);
            transform.DetachChildren();
        }
        Destroy(gameObject);
    }

    private void OnPickup()
    {
        if (GlobalInfo.Instance.heldItem != null && GlobalInfo.Instance.heldItem.Equals(gameObject))
        {
            GameObject erdSplash = Instantiate(earthSplash, transform.position, Quaternion.identity);
            erdSplash.transform.SetParent(GlobalInfo.Instance.playerTrans);
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (PlayerInputManager.Instance != null)
            PlayerInputManager.Instance.onItemPickup -= OnPickup;
    }
}
