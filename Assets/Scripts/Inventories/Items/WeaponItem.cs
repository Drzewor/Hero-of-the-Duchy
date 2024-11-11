using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    [System.Serializable] 
    public enum statDamageBonus
    {
        Strength = 0,
        Dexterity = 1,
        Charisma = 2,
        none = 3
    }

    [CreateAssetMenu(fileName = "Weapon", menuName = "RPG/Weapon")]
    public class WeaponItem : EquippableItem
    {
        public GameObject weaponPrefab;
        public float weaponDamage = 0;
        public float npcAttackRange = 2;
        public statDamageBonus statDamageBonus;
        public Attack[] attacks;

        public override string GetItemType()
        {
            return EquipmentType.ToString();
        }

        public override string GetDescription()
        {
            sb.Length = 0;
            sb.Append("Weapon Damage ");
            sb.Append(weaponDamage);
            AddStats(MaxHealthBonus,"MaxHealth");
            AddStats(MaxManaBonus,"MaxMana");
            AddStats(StrengthBonus,"Strength");
            AddStats(DexterityBonus,"Dexterity");
            AddStats(CharismaBonus,"Charisma");
            AddStats(WeaponArmourBonus,"Weapon Armour");
            AddStats(MagicArmourBonus,"Magic Armour");
            AddStats(ProjectileArmourBonus,"Projectile Armour");
            AddStats(ArmourPiercingBonus,"Armour Piercing");

            AddStats(MaxHealthPercentBonus,"Max Health", true);
            AddStats(MaxManaPercentBonus,"Max Mana", true);
            AddStats(StrengthPercentBonus,"Strength", true);
            AddStats(DexterityPercentBonus,"Dexterity", true);
            AddStats(CharismaPercentBonus,"Charisma", true);
            AddStats(WeaponArmourPercentBonus,"Weapon Armour", true);
            AddStats(MagicArmourPercentBonus,"Magic Armour", true);
            AddStats(ProjectileArmourPercentBonus,"Projectile Armour", true);
            AddStats(ArmourPiercingPercentBonus,"Armour Piercing", true);

            return sb.ToString();
        }

    }

}
