using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayerButton : MonoBehaviour
{
    public void KillThePlayer()
    {
        if (GlobalInfo.Instance.playerTrans.TryGetComponent(out PlayerHealth ph))
            ph.Die();
    }
}
