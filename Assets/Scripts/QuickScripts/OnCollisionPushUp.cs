using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPushUp : MonoBehaviour
{
    public float knockbackForce = 100f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Rigidbody rb))
        {
            Vector3 dir = collision.transform.position - collision.GetContact(0).point;
            rb.AddForce((dir + Vector3.up) * knockbackForce, ForceMode.Impulse);
        }
    }
}