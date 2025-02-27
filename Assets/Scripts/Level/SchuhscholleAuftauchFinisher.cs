using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchuhscholleAuftauchFinisher : MonoBehaviour
{
    public MoveInDirection moveInDirectionRef;
    public float targetYPos = -3.8f;

    private void Update()
    {
        if (transform.position.y > targetYPos)
        {
            moveInDirectionRef.enabled = false;
            this.enabled = false;
        }
    }
}
