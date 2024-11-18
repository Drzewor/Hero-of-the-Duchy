using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class NPCLoot : MonoBehaviour
{
    [SerializeField] public int expBounty; 
    [SerializeField] List<ItemLoot> items;
    [SerializeField] ItemDropper itemDropper;

    public void SpawnLoot()
    {
        if(items.Count == 0) return;

        foreach(ItemLoot loot in items)
        {
            itemDropper.DropItem(loot.item, loot.amount);
        }
    }

    public void GrantExpBounty(CharacterExperience experience)
    {
        experience.AddExp(expBounty);
    }


    [Serializable]
    private class ItemLoot
    {
        public Item item;
        public int amount;
    }
}
