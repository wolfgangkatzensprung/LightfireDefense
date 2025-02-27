using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormMovement : MonoBehaviour
{
    Rigidbody rb;
    Animator anim;
    EnemyHealth eh;
    Transform playerTrans;

    Vector3 eulerAngleVelocity = new Vector3(100, 0, 0);
    public float speed = 7f;
    public float knockbackStrength = 3f;

    Vector3 startPosition;
    float maxDistance = 35f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        playerTrans = GlobalInfo.Instance.playerTrans;
        eh = GetComponent<EnemyHealth>();

        startPosition = transform.position;

        eh.onDamaged += Knockback;
    }

    private void FixedUpdate()
    {
        if (anim.GetBool("isIdle") && Vector3.Distance(transform.position, startPosition) > maxDistance)
        {
            IdleMove();
        }
        else if (anim.GetBool("isChasing"))
        {
            Chase();
        }
    }

    private void IdleMove()
    {
        Vector3 moveDirection = startPosition - transform.position;
        rb.AddForce(speed * moveDirection * Time.deltaTime, ForceMode.Force);
    }

    private void Knockback(Damage.DamageType dmgType)
    {
        anim.Play("Knockback");
        Vector3 knockbackDirection = transform.position - playerTrans.position;
        rb.velocity = Vector3.zero;
        rb.AddForce(knockbackDirection * knockbackStrength, ForceMode.Impulse);
    }

    private void Chase()
    {
        Debug.Log("Chasing Player");

        LookAtPlayer();
        MoveTo(playerTrans.position);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(playerTrans);
    }

    //private void Rotate()
    //{
    //    Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.fixedDeltaTime);
    //    rb.MoveRotation(rb.rotation * deltaRotation);
    //}

    private void MoveTo(Vector3 targetPosition)
    {
        Vector3 targetVelocity = (targetPosition - transform.position).normalized * speed;
        rb.velocity = targetVelocity;
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        eh.onDamaged -= Knockback;
    }
}
