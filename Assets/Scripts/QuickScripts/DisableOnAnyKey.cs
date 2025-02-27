using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAnyKey : MonoBehaviour
{
    public float startDelay = 0f;
    float timer = 0f;

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        if (timer > startDelay && Input.anyKeyDown)
        {
            gameObject.SetActive(false);
        }
    }
}
