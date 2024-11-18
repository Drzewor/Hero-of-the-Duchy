using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

public class ItemStash : ItemContainer, IInteractable
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private Transform snapingPosition;
    [SerializeField] private string raycastText;

    protected override void OnValidate()
    {
        if(itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>(includeInactive: true);
        }
    }

    public void Iteraction(GameObject player)
    {
        if(itemsParent == null) return;
        PlayerStateMachine stateMachine = player.GetComponent<PlayerStateMachine>();
        player.transform.position = snapingPosition.position;
        player.transform.rotation = snapingPosition.rotation;
        stateMachine.SwitchState(new PlayerItemStashState(stateMachine, itemsParent, this));
    }

    public void HandleRaycast(GameObject player)
    {
        TMP_Text interactionText = player.GetComponent<PlayerStateMachine>().InteractionText;
        interactionText.enabled = true;
        if(string.IsNullOrEmpty(raycastText))
        {
            interactionText.text = $"Press F to open {name}";
            return;
        }
        else
        {
            interactionText.text = $"{raycastText}";
        }
    }
}
