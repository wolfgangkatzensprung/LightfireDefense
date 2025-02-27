using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOrbTextOnCollect : MonoBehaviour
{
    [TextArea(minLines:2, maxLines:15)] public string orbCollectedTextString;

    public void ShowOrbCollectedUI()
    {
        UIManager.Instance.ShowPopupTextAndPause(orbCollectedTextString);
    }
}