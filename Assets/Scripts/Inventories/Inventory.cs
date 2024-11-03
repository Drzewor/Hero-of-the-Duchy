using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

public class Inventory : ItemContainer
{
    [SerializeField] Item[] startingItems;
    [SerializeField] Transform itemsParent;


    protected override void Awake() 
    {
        base.Awake();
        SetStartingItems();
    }

    protected override void OnValidate() 
    {
        if(itemsParent != null)
        {
            itemSlots = itemsParent.GetComponentsInChildren<ItemSlot>(includeInactive: true);
        }
    }

    private void SetStartingItems()
    {
        Clear();
        for(int i = 0; i < startingItems.Length; i++)
        {
            AddItem(startingItems[i].GetCopy());
        }
    }

    
}
