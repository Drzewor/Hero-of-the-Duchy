using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Inventories
{
    [CreateAssetMenu(fileName = "Armor", menuName = "RPG/Armor")]
    public class ArmorItem : EquippableItem
    {
        public GameObject EquipmentPrefab;
        public GameObject SecondEquipmentPrefab = null;

    }
}

