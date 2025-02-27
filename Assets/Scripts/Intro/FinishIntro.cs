using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishIntro : MonoBehaviour
{
    [Tooltip("Reference of IntroHandlerAndTrigger")]
    public IntroHandlerAndTrigger ihatReference;

    float introSkipTimer = 0f;

    private void Update()
    {
        if (Input.GetButton("Escape") || Input.GetButton("Ready") || Input.GetButton("Jump"))
        {
            introSkipTimer += Time.unscaledDeltaTime;

            if (introSkipTimer > .2f)
            {
                introSkipTimer = 0f;
                ihatReference.SkipIntro();
                gameObject.SetActive(false);
                return;
            }
        }
    }

    public void EndIntro()
    {
        GameController.Instance.FinishIntro();
    }
}