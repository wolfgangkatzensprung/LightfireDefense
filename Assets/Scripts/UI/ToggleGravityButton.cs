using ECM.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGravityButton : MonoBehaviour
{
    CharacterMovement cm;

    bool antiGravityModeEnabled;

    private void Start()
    {
        cm = GameObject.Find("PLAYER").GetComponent<CharacterMovement>();
    }

    public void ToggleGodmode()
    {
        antiGravityModeEnabled = !antiGravityModeEnabled;

        if(antiGravityModeEnabled)
        {
            cm.useGravity = false;
        }
        else if(!antiGravityModeEnabled)
        {
            cm.useGravity = true;
        }
    }
}
