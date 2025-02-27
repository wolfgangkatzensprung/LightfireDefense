using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncySurface : MonoBehaviour
{
    public float bounceStrength = 23f;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (collision.collider.transform.TryGetComponent(out Rigidbody rb))
                rb.AddForce(Vector3.up * bounceStrength, ForceMode.Impulse);
        }

    }
}
