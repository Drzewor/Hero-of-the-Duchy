using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class ItemStash : ItemContainer, IInteractable
{
    [SerializeField] Transform itemsParent;

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
        stateMachine.SwitchState(new PlayerItemStashState(stateMachine, itemsParent, this));
    }
}
