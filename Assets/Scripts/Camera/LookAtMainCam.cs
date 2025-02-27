using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMainCam : MonoBehaviour
{
    Transform mainCamTrans;

    private void Start()
    {
        if (GlobalInfo.Instance != null)
            mainCamTrans = GlobalInfo.Instance.mainCam;
        else
            mainCamTrans = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamTrans.rotation * Vector3.forward,
            mainCamTrans.rotation * Vector3.up);
    }
}
