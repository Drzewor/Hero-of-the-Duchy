using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public abstract class ItemContainer : MonoBehaviour, IItemContainer, ISaveable
    {
        [SerializeField] protected ItemSlot[] itemSlots;
        [SerializeField] protected Item[] startingItems;
        [SerializeField] public ItemDatabase itemDatabase;
        public event Action<BaseItemSlot> OnPointerEnterEvent;
        public event Action<BaseItemSlot> OnPointerExiEvent;
        public event Action<BaseItemSlot> OnRightClickEvent;
        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;

        protected virtual void OnValidate()
        {
            itemSlots = GetComponentsInChildren<ItemSlot>(includeInactive: true);
        }

        protected virtual void Awake() 
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
                itemSlots[i].OnPointerExitEvent += slot => OnPointerExiEvent(slot);
                itemSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
                itemSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
                itemSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
                itemSlots[i].OnDragEvent += slot => OnDragEvent(slot);
                itemSlots[i].OnDropEvent += slot => OnDropEvent(slot);
            }
        }
        
        public virtual bool CanAddItem(Item item, int amount = 1)
        {
            int freeSpaces = 0;

            foreach(ItemSlot itemSlot in itemSlots)
            {
                if(itemSlot.Item == null || itemSlot.Item.ID == item.ID)
                {
                    freeSpaces += item.MaximumStacks - itemSlot.Amount;
                }
            }
            return freeSpaces >= amount;
        }

        public virtual bool AddItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if(itemSlots[i].Item == null || itemSlots[i].CanAddStack(item))
                {
                    itemSlots[i].Item = item;
                    itemSlots[i].Amount++;
                    return true;
                }
            }

            for (int i = 0; i < itemSlots.Length; i++)
            {
                if(itemSlots[i].Item == null)
                {
                    itemSlots[i].Item = item;
                    itemSlots[i].Amount++;
                    return true;
                }
            }
            return false;
        }
        public virtual bool RemoveItem(Item item)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                if(itemSlots[i].Item == item)
                {
                    itemSlots[i].Amount--;
                    return true;
                }
            }
            return false;
        }

        
        public virtual bool HasItem(string itemID)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item item = itemSlots[i].Item;
                if(item != null && item.ID == itemID)
                {
                    return true;
                }
            }
            return false;
        }

        public virtual Item RemoveItem(string itemID)
        {
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item item = itemSlots[i].Item;
                if(item != null && item.ID == itemID)
                {
                    itemSlots[i].Amount--;
                    return item;
                }
            }
            return null;
        }

        public virtual int ItemCount(string itemID)
        {
            int number = 0;
            for (int i = 0; i < itemSlots.Length; i++)
            {
                Item item = itemSlots[i].Item;
                if(item != null && item.ID == itemID)
                {
                    number += itemSlots[i].Amount;
                }
            }
            return number;
        }

        public virtual void Clear()
        {
            for(int i = 0; i < itemSlots.Length; i++)
            {
                itemSlots[i].Item = null;
            }
        }

        protected void SetStartingItems()
        {
            Clear();
            for(int i = 0; i < startingItems.Length; i++)
            {
                AddItem(startingItems[i].GetCopy());
            }
        }

        public object CaptureState()
        {
            var saveData = new ItemContainerSaveData(itemSlots.Length);
            for (int i = 0; i < saveData.SavedSlots.Length; i++)
            {
                ItemSlot itemSlot = itemSlots[i];
                if(itemSlot.Item == null) 
                {
                    saveData.SavedSlots[i] = null;
                }
                else
                {
                    saveData.SavedSlots[i] = new ItemSlotSaveData(itemSlot.Item.ID, itemSlot.Amount);
                }
            }

            return saveData;
        }

        public void RestoreState(object state)
        {
            var saveData = (ItemContainerSaveData)state;

            if(saveData == null) return;

            Clear();

            for (int i = 0; i < saveData.SavedSlots.Length; i++)
            {
                ItemSlot itemSlot = itemSlots[i];
                ItemSlotSaveData savedSlot = saveData.SavedSlots[i];

                if(savedSlot == null)
                {
                    itemSlot.Item = null;
                    itemSlot.Amount = 0;
                }
                else
                {
                    itemSlot.Item = itemDatabase.GetItemCopy(savedSlot.ItemID);
                    itemSlot.Amount = savedSlot.Amount;
                }
            }
        }

    }

}
