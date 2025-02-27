using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelDots : MonoBehaviour
{
    public PlayerExp.ExpType expType;

    private void Awake()
    {
        InitializeLevelDots();
    }

    private void InitializeLevelDots()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.TryGetComponent(out Image img))
            {
                img.enabled = false;
                if (child.childCount > 0 && child.GetChild(0).TryGetComponent(out Image lineImg))
                {
                    lineImg.enabled = false;
                    if (child.childCount > 1 && child.GetChild(1).TryGetComponent(out Image innerDot))
                        innerDot.enabled = false;
                }
            }
        }
    }

    private void OnEnable()
    {
        int level = PlayerExp.Instance.level[(int)expType];

        int length = Mathf.Min(transform.childCount, level);

        for (int i = 0; i < length; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.TryGetComponent(out Image img))
            {
                img.enabled = true;
                if (child.childCount > 0 && child.GetChild(0).TryGetComponent(out Image lineImg))
                {
                    lineImg.enabled = true;
                    if (child.childCount > 1 && child.GetChild(1).TryGetComponent(out Image innerDot))
                        innerDot.enabled = true;
                }
            }
        }
    }
}
