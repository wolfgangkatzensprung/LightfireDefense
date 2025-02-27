using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECM.Components;

public class GravityJump : MonoBehaviour
{
    PlayerMovementController pmc;
    Rigidbody rb;
    CharacterMovement cm;

    public ParticleSystem gravityJumpParticles;

    [Tooltip("Mana Cost for GravityJump")]
    public float manaCost = 25f;

    public float gravityJumpForce = 11f;
    float gravityJumpTimer = 0f;    // descending timer
    public float gravityJumpTime = 3f;

    bool isGravityJumping;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pmc = GetComponent<PlayerMovementController>();
        cm = GetComponent<CharacterMovement>();

        PlayerInputManager.Instance.onUlt += TryStartGravityJump;

        gravityJumpParticles.Stop();
    }

    private void Update()
    {
        if (isGravityJumping)
            gravityJumpTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (gravityJumpTimer > 0f)
        {
            rb.AddForce(Vector3.up * gravityJumpForce, ForceMode.Acceleration);
            Debug.Log("accelerate Gravity Jump");
        }
        else if (!cm.useGravity)
        {
            StopGravityJump();
        }
    }


    void TryStartGravityJump()
    {
        Debug.Log($"EarthKey {SphereKeys.HasKey(SphereKeys.KeyType.Earth)}, Mana {PlayerMana.Instance.currentMana}");
        if (SphereKeys.HasKey(SphereKeys.KeyType.Earth) && PlayerMana.Instance.currentMana >= 25f)
        {
            PlayerMana.Instance.UseMana(manaCost);
            StartGravityJump();
        }
    }

    private void StartGravityJump()
    {
        Debug.Log("Gravity Jump");
        gravityJumpParticles.Play();
        isGravityJumping = true;
        cm.useGravity = false;
        pmc.jump = true;
        gravityJumpTimer = 3f;
    }
    private void StopGravityJump()
    {
        cm.useGravity = true;
        gravityJumpParticles.Stop();
        isGravityJumping = false;
    }
}