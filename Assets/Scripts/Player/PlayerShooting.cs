using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerSpells))]
public class PlayerShooting : Singleton<PlayerShooting>
{
    MainCamRaycast mcr;

    public GameObject[] projectilePrefabs = new GameObject[1];
    public LayerMask enemiesLayer;

    Transform firePoint;
    PlayerDamageCone pdc;
    ParticleSystem flamethrowerParticles;
    Collider pdcCollider;
    AirAttackLineRenderer airAttackLR;

    public int killCount = 0;

    internal int currentBonusDmg = 0;
    internal const int maxBonusDmg = 5;

    bool isShootingRaycast;
    internal int damageMultiplier = 3;
    internal bool wandEquipped;

    internal bool canShoot = true;

    enum ShotType
    {
        Projectile, // Earth = 0, Water = 2
        DamageCone, // Fire = 1
        Raycast     //  Air = 3
    }
    ShotType shotType;

    private void Start()
    {
        mcr = MainCamRaycast.Instance;
        firePoint = GlobalInfo.Instance.firePoint;
        pdc = firePoint.GetChild(0).GetComponent<PlayerDamageCone>();
        pdcCollider = pdc.GetComponent<Collider>();
        flamethrowerParticles = pdc.GetComponentInChildren<ParticleSystem>();
        airAttackLR = firePoint.GetComponentInChildren<AirAttackLineRenderer>();        

        flamethrowerParticles.enableEmission = false;
        flamethrowerParticles.Clear();

        if (SphereKeys.HasKey(SphereKeys.KeyType.Fire))
        {
            currentBonusDmg = maxBonusDmg;
        }

        PlayerInputManager.Instance.onAttack += TryShoot;
        PlayerInputManager.Instance.onStopAttack += DisableDamageCone;
        PlayerInputManager.Instance.onStopAttack += DisableShootRaycast;
        ElementalScroll.Instance.onEleChange += SwapShotTypeByEleChange;
        ElementalScroll.Instance.onEleChange += DisableDamageCone;
        ElementalScroll.Instance.onEleChange += DisableShootRaycast;
    }

    private void Update()
    {
        if (isShootingRaycast)
        {
            ShootRaycast();
        }
    }

    private void SwapShotTypeByEleChange()
    {
        switch(ElementalScroll.Instance.selectionIndex)
        {
            case 0:
                shotType = ShotType.Projectile;
                break;
            case 1:
                shotType = ShotType.DamageCone;
                break;
            case 2:
                shotType = ShotType.Projectile;
                break;
            case 3:
                shotType = ShotType.Raycast;
                break;
        }
        //Debug.Log($"ShotType: {shotType.ToString()}");
    }

    void TryShoot()
    {
        //Debug.Log($"TryShoot - canShoot: {canShoot}, inMenu: {GlobalInfo.Instance.inMenu}, wandEquipped: {wandEquipped}, currentMana: {PlayerMana.Instance.currentMana}, attackManaCost: {PlayerMana.Instance.attackManaCost}");
        if (canShoot && !GlobalInfo.inMenu && wandEquipped)
        {
            //Debug.Log("Can Shoot");
            if (PlayerMana.Instance.currentMana > PlayerMana.Instance.attackManaCost)
                Shoot();
            else
            {
                SoundManager.Instance.PlayNonspacialSound(SoundManager.Sound.Error);
            }
        }
    }

    void Shoot()
    {
        PlayerMana.Instance.UseMana_Attack();

        if (shotType.Equals(ShotType.Projectile))
        {
            ShootProjectile();
            return;
        }
        else if (shotType.Equals(ShotType.DamageCone))
        {
            EnableDamageCone();
            PlayerMana.Instance.UseMana_Attack();
            return;
        }
        else if (shotType.Equals(ShotType.Raycast))
        {
            EnableShootRaycast();
            return;
        }
    }

    private void EnableShootRaycast()
    {
        isShootingRaycast = true;
        SoundManager.Instance.PlayAttackSoundscape(true);
    }

    private void DisableShootRaycast()
    {
        SoundManager.Instance.StopAttackSoundscape();
        isShootingRaycast = false;
    }

    private void ShootRaycast()
    {
        if (mcr.aimingAtEnemy)
        {
            RaycastHit hit = mcr.GetLastEnemyHit();
            if (hit.transform.TryGetComponent(out IDamageable enemyHealthComponent))
            {
                Damage.DamageType dmgType = ElementalScroll.Instance.GetCurrentDamageType();
                int dmg = PlayerExp.Instance.level[(int)dmgType];

                enemyHealthComponent.TryApplyDamage(dmg + currentBonusDmg, dmgType);

                float capsuleSize = 10f;
                if (Physics.CapsuleCast(hit.transform.position, transform.position - Vector3.left * capsuleSize, 1f, Vector3.forward, out RaycastHit leftHit, capsuleSize, enemiesLayer))
                {
                    if (leftHit.transform.TryGetComponent(out EnemyHealth leftEh))
                    {

                        leftEh.TryApplyDamage(dmg + currentBonusDmg, dmgType);
                    }
                }
                if (Physics.CapsuleCast(hit.transform.position, transform.position + Vector3.right * capsuleSize, 1f, Vector3.forward, out RaycastHit rightHit, capsuleSize, enemiesLayer))
                {
                    if (rightHit.transform.TryGetComponent(out EnemyHealth rightEh))
                    {
                        rightEh.TryApplyDamage(dmg + currentBonusDmg, dmgType);
                    }
                }
            }
        }
        airAttackLR.StartLineRenderer();

        //GameObject vfx = Instantiate(projectilePrefabs[ElementalScroll.Instance.selectionIndex], firePoint.position, firePoint.rotation);
        //Debug.Log($"{vfx.name} instantiated!");
    }

    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefabs[ElementalScroll.Instance.selectionIndex], firePoint.position, Quaternion.identity);

        if (projectile.TryGetComponent(out Rigidbody rb))
        {
            if (rb.TryGetComponent(out Projectile proj))
            {
                Vector3 direction = GlobalInfo.Instance.mainCam.transform.forward;
                Vector3 earthProjectileFactor = Vector3.zero;

                if (TryGetComponent(out EarthProjectile ep))
                {
                    earthProjectileFactor = Vector3.up * 3f;
                }

                rb.velocity = proj.speed * (direction + earthProjectileFactor);
            }
        }
    }

    private void EnableDamageCone()
    {
        SoundManager.Instance.PlayAttackSoundscape(false);
        pdcCollider.enabled = true;
        flamethrowerParticles.enableEmission = true;
    }

    private void DisableDamageCone()
    {
        SoundManager.Instance.StopAttackSoundscape();
        pdcCollider.enabled = false;
        flamethrowerParticles.enableEmission = false;
    }

    internal void IncreaseKillCount(int add)
    {
        killCount += add;
        UIManager.Instance.SetKillCountText(killCount.ToString());
    }
}
