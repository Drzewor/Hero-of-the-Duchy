using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

namespace RPG.Inventories
{
    public class Pickup : MonoBehaviour, IInteractable
    {
        public Item item;
        public int amount;
        public PickupSpawner spawner;
        private Inventory inventory;

        public void Interaction(GameObject player)
        {
            inventory = player.GetComponent<PlayerStateMachine>().InventoryManager.inventory;
            Item itemCopy = item.GetCopy();
            for(int i = 0; i < amount;)
            {
                if(inventory.CanAddItem(item, amount))
                {
                    if(inventory.AddItem(itemCopy))
                    {
                        amount--;
                        if(amount == 0)
                        {
                            if(spawner != null) spawner.isPicked = true;
                            
                            Destroy(gameObject);
                        }
                    }
                    else
                    {
                        itemCopy.Destroy();
                    }
                }
            } 
        }

        public void HandleRaycast(GameObject player)
        {
            TMP_Text interactionText = player.GetComponent<PlayerStateMachine>().InteractionText;
            interactionText.enabled = true;
            if(amount == 1)
            {
                interactionText.text = $"Press F to pickup {item.ItemName}";
                return;
            }
            else
            {
                interactionText.text = $"Press F to pickup {item.ItemName} x{amount}";
                return;            
            }

        }
    }

}
