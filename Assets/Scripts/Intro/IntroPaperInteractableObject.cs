using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroPaperInteractableObject : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        UIManager.Instance.ShowIntroPaper();
    }
}