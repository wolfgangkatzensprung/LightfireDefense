using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : Projectile
{
    private void Start()
    {
        Destroy(gameObject, selfDestructionTime);
    }
}
