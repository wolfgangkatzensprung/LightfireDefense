using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbProjectileHitEffect : EnemyProjectileHitEffect
{
    [Tooltip("Player will be pushed towards (0,0,0)")]
    public float pushForce = 100f;

    [Tooltip("When Player Y Position is below this value, he will not be pushed")]
    public float heightThreshold = 50f;

    //private void PushPlayerTowardMiddleOfMap(Collision collision)
    //{
    //    Vector3 dir = Vector3.zero - collision.transform.position;
    //    Vector3 direction = new Vector3(dir.x, 5, dir.z).normalized;
    //    collision.rigidbody.AddForce(direction * pushForce, ForceMode.Impulse);
    //}

    //public override void ApplyHitEffect(Collision collision)
    //{
    //    if (collision.transform.position.y > heightThreshold)
    //    {
    //        PushPlayerTowardMiddleOfMap(collision);
    //    }

    //    base.ApplyHitEffect(collision);
    //}
}
