using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;

namespace RPG.Inventories
{
    public class Inventory : ItemContainer
    {
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
    }
}

