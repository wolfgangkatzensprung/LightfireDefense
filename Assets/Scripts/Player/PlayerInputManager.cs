using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : Singleton<PlayerInputManager>
{
    Transform itemHolder;

    Ray ray;
    RaycastHit hit;

    [Tooltip("Maximum interaction and pickup radius")]
    public float interactionDistance = 10f;

    public delegate void InteractDelegate();
    public InteractDelegate onInteract;

    public delegate void SpellCastDelegate();
    public SpellCastDelegate onSpellCast;

    public delegate void BuildDelegate();
    public BuildDelegate onBuild;
    public delegate void CancelBuildDelegate();
    public CancelBuildDelegate onCancelBuild;

    public delegate void ItemPickupDelegate();
    public ItemPickupDelegate onItemPickup;

    public delegate void ItemDropDelegate();
    public ItemDropDelegate onItemDrop;

    public delegate void UltiDelegate();
    public UltiDelegate onUlt;  // on ultimate ability activation

    public delegate void MouseScrollUpDelegate(int mouseScrollDirection);
    public MouseScrollUpDelegate onMouseScroll;

    public delegate void AttackDelegate();
    public AttackDelegate onAttack;
    float attackTimer = 0f;
    float attackCooldown = .3f;

    public delegate void StopAttackDelegate();
    public AttackDelegate onStopAttack;

    public delegate void PlayerReadyDelegate();
    public PlayerReadyDelegate onPlayerRdy;

    private void Start()
    {
        itemHolder = GlobalInfo.Instance.itemHolder;
        PlayerHealth.Instance.onDeath += TryDropItem;
    }
    void Update()
    {
        if (UIManager.Instance == null)
            return;

        HandleItemsAndInteractions();

        HandleSpellInput();

        if (Input.GetButtonDown("Escape") || Input.GetButtonDown("Tab") || Input.GetMouseButtonDown(1))
        {
            onCancelBuild?.Invoke();
        }
        else if (Input.GetButtonDown("Build"))
        {
            onBuild?.Invoke();
        }

        HandleAttackInput();

        HandleNumbersInput();

        if (Input.GetButtonDown("Ulti"))
        {
            Debug.Log("Ult");
            onUlt?.Invoke();
        }

        if (Input.GetButtonDown("Ready"))
        {
            Debug.Log("Ready");
            onPlayerRdy?.Invoke();
        }
    }

    private void HandleAttackInput()
    {
        if (Input.GetButtonUp("Attack"))
        {
            onStopAttack?.Invoke();
        }
        else if (Input.GetButtonDown("Attack"))
        {
            onAttack?.Invoke();
            attackTimer = 0f;
        }
        else if (Input.GetButton("Attack"))
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > attackCooldown)
            {
                onAttack?.Invoke();
                attackTimer = 0f;
            }
        }
    }

    private static void HandleNumbersInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ElementalScroll.Instance.SetElement(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ElementalScroll.Instance.SetElement(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ElementalScroll.Instance.SetElement(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ElementalScroll.Instance.SetElement(3);
        }
    }

    private void HandleItemsAndInteractions()
    {
        if(Camera.main != null)     // nullcheck ist nur zum Ueberschreiben der MainCam durch FreeCam
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (itemHolder.childCount > 0 && itemHolder.GetChild(0).TryGetComponent(out IInteractable interactableComponent))
        {
            //UIManager.Instance.ShowInteractHint(hit);
            if (Input.GetButtonDown("Interact"))
            {
                interactableComponent.Interact();
                return;
            }
        }
       // else UIManager.Instance.TryHideInteractHint();

        if (Physics.Raycast(ray, out hit, interactionDistance, GlobalInfo.Instance.interactableLayer))
        {
            Debug.Log($"Ray hit Interactable: {hit.collider.name}");
            if (hit.transform.TryGetComponent(out IInteractable interactable))
            {
                UIManager.Instance.ShowInteractHint(hit);

                if (Input.GetButtonDown("Interact"))
                {
                    if (itemHolder.childCount != 1)
                    {
                        interactable.Interact();
                    }

                    onInteract?.Invoke();
                }
            }
           else UIManager.Instance.TryHideInteractHint();
        }
        else UIManager.Instance.TryHideInteractHint();

        if (Physics.Raycast(ray, out hit, interactionDistance, GlobalInfo.Instance.itemLayer))
        {
            Debug.Log($"Ray hit Item: {hit.collider.name}");

            if (hit.transform.TryGetComponent(out IEquipable itemComponent))
            {
                if (GlobalInfo.Instance.heldItem == null)
                    UIManager.Instance.ShowItemHint(hit);

                if (Input.GetButtonDown("Item"))
                {
                    if (itemHolder.childCount == 0)
                    {
                        Debug.Log($"Picking up item {hit.collider}");
                        itemComponent.Equip();
                        onItemPickup?.Invoke();
                        return;
                    }
                }
            }
            else UIManager.Instance.TryHideItemHint();

            if (hit.transform.TryGetComponent(out IInteractable interactableItemComponent))        // fuer Items mit Interactable Component, zB Turret oder Trap
            {
                UIManager.Instance.ShowInteractHint(hit);

                if (Input.GetButtonDown("Interact"))
                {
                    if (itemHolder.childCount != 1)
                    {
                        interactableItemComponent.Interact();
                    }

                    onInteract?.Invoke();
                }
            }
            else UIManager.Instance.TryHideInteractHint();
        }
        else UIManager.Instance.TryHideItemHint();

        if (Input.GetButtonDown("Item"))
        {
            TryDropItem();
        }
    }

    private void TryDropItem()
    {
        if (itemHolder.childCount > 0 && itemHolder.GetChild(0).TryGetComponent(out IEquipable itemComponent))
        {
            Debug.Log($"Dropping item.");
            itemComponent.Unequip();
            onItemDrop?.Invoke();
        }
    }

    private void HandleSpellInput()
    {
        if (Input.GetButtonUp("Spell"))
        {
            onSpellCast?.Invoke();
        }
        else
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                onMouseScroll?.Invoke(1);
                return;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                onMouseScroll?.Invoke(-1);
                return;
            }
        }
    }
}