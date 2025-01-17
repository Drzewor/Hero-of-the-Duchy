using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TMPro;

namespace RPG.Inventories
{
    public class ItemSlot : BaseItemSlot, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
    {

        public event Action<BaseItemSlot> OnBeginDragEvent;
        public event Action<BaseItemSlot> OnEndDragEvent;
        public event Action<BaseItemSlot> OnDragEvent;
        public event Action<BaseItemSlot> OnDropEvent;

        public override bool CanAddStack(Item item, int amount = 1)
        {
            return base.CanAddStack(item, amount) && Amount + amount <= item.MaximumStacks;;
        }

        public override bool CanReceiveItem(Item item)
        {
            return true;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(OnBeginDragEvent != null)
            {
                OnBeginDragEvent(this);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(OnEndDragEvent != null)
            {
                OnEndDragEvent(this);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(OnDragEvent != null)
            {
                OnDragEvent(this);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(OnDropEvent != null)
            {
                OnDropEvent(this);
            }
        }
    }

}
