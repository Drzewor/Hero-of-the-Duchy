using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    public abstract class UsableItemEffect : ScriptableObject
    {
        public abstract void ExecuteEffect(UsableItem parentItem, InventoryManager character);

        public abstract string GetDescription();
    }
}

