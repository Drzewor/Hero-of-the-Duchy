using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class Pickup : MonoBehaviour, IInteractable
{
    public Item item;
    public int amount;
    public PickupSpawner spawner;
    private Inventory inventory;
    
    public void Iteraction(GameObject player)
    {
        inventory = player.GetComponent<PlayerStateMachine>().Character.inventory;
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
}
