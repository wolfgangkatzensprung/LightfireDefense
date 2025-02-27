using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    // Settings
    public Vector2 velocity;
    public bool returnToStart = false;
    public float maxDistance = 50f;

    // Cache
    bool moving = false;
    bool returning = false;
    float speed = 1f;       // multiplikator fuer velocity
    Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;
        moving = true;
    }

    private void FixedUpdate()
    {
        if (moving)
        {
            transform.position += (Vector3)(velocity * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPosition) > maxDistance)
            {
                returning = true;
                moving = false;
            }
        }
        else if (returning)
        {
            transform.position += (Vector3)(-velocity * speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, startPosition) < 1f)
            {
                returning = false;
                moving = true;
            }
        }
    }
}
