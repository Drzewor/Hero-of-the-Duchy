using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Saving;
using UnityEngine;

namespace RPG.Inventories
{
    public class EquipmentPanel : MonoBehaviour
    {
        [SerializeField] Transform equipmentSlotsParent;
        [SerializeField] public EquipmentSlot[] equipmentSlots;
        public event Action<BaseItemSlot> OnPointerEnterEvent;
        public event Action<BaseItemSlot> OnPointerExiEvent;
        public event Action<BaseItemSlot> OnRightClickEvent;
        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;

        private void Start() 
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                equipmentSlots[i].OnPointerEnterEvent += slot => OnPointerEnterEvent(slot);
                equipmentSlots[i].OnPointerExitEvent += slot => OnPointerExiEvent(slot);
                equipmentSlots[i].OnRightClickEvent += slot => OnRightClickEvent(slot);
                equipmentSlots[i].OnBeginDragEvent += slot => OnBeginDragEvent(slot);
                equipmentSlots[i].OnEndDragEvent += slot => OnEndDragEvent(slot);
                equipmentSlots[i].OnDragEvent += slot => OnDragEvent(slot);
                equipmentSlots[i].OnDropEvent += slot => OnDropEvent(slot);
            }
        }    
        
        private void OnValidate() 
        {
            equipmentSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
        }

        public bool AddItem(EquippableItem item, out EquippableItem previousItem)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if(equipmentSlots[i].EquipmentType == item.EquipmentType)
                {
                    previousItem = (EquippableItem)equipmentSlots[i].Item;
                    equipmentSlots[i].Item = item;
                    equipmentSlots[i].Amount = 1;
                    return true;
                }
                
            }
            previousItem = null;
            return false;   
        }

        public bool RemoveItem(EquippableItem item)
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                if(equipmentSlots[i].Item == item)
                {
                    equipmentSlots[i].Item = null;
                    return true;
                }
            }
            return false;   
        }

        public virtual void Clear()
        {
            for(int i = 0; i < equipmentSlots.Length; i++)
            {
                if(equipmentSlots[i].Item == null) continue;

                RemoveItem((EquippableItem)equipmentSlots[i].Item);
            }
        }

        public virtual bool HasItem(string ItemID)
        {
            for(int i = 0; i < equipmentSlots.Length; i++)
            {
                if(equipmentSlots[i].Item == null) continue;

                if(equipmentSlots[i].Item.ID == ItemID)
                {
                    return true;
                }
            }
            return false;
        }
    }

}
