using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceAtStart : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 velocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(velocity, ForceMode2D.Impulse);
    }
}
