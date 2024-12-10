using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEventHandler : MonoBehaviour
{
    private Inventory inventory;
    private void Start() 
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryManager>().GetInventory();
    }

    public void TakeItem(Item item)
    {
        inventory.RemoveItem(item); 
    }

    public void GiveItem(Item item)
    {
        inventory.AddItem(item);
    }
}
