using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavingGameUI : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        SaveSystem.onStartSavingGame += PlaySavingSequence;
    }

    private void PlaySavingSequence()
    {
        anim.Play("Save");
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        return;
#endif
        SaveSystem.onStartSavingGame -= PlaySavingSequence;
    }
}
