using System.Collections;
using System.Collections.Generic;
using RPG.CharacterStats;
using UnityEngine;

public enum EquipmentType
{
    Helmet,
    Armour,
    Gloves,
    Boots,
    MainHand,
    OffHand,
    Amulet,
    Accesory1,
    Accesory2,
}

[CreateAssetMenu(fileName = "New EquippableItem", menuName = "RPG/New EquippableItem", order = 0)]
public class EquippableItem : Item
{
        public EquipmentType EquipmentType;
        [Space]
        public int MaxHealthBonus;
        public int MaxManaBonus;
        public int StrengthBonus;
        public int DexterityBonus;
        public int CharismaBonus;
        public int WeaponArmourBonus;
        public int MagicArmourBonus;
        public int ProjectileArmourBonus;
        public int ArmourPiercingBonus;
        [Space]
        public float MaxHealthPercentBonus;
        public float MaxManaPercentBonus;
        public float StrengthPercentBonus;
        public float DexterityPercentBonus;
        public float CharismaPercentBonus;
        public float WeaponArmourPercentBonus;
        public float MagicArmourPercentBonus;
        public float ProjectileArmourPercentBonus;
        public float ArmourPiercingPercentBonus;

    public override Item GetCopy()
    {
        return Instantiate(this);
    }

    public override void Destroy()
    {
        Destroy(this);
    }

    public void Equip(PlayerCharacter c)
    {
        if(MaxHealthBonus != 0) 
            c.MaxHealth.AddModifier(new StatModifier(MaxHealthBonus, statModType.Flat, this)); 
        if(MaxManaBonus != 0) 
            c.MaxMana.AddModifier(new StatModifier(MaxManaBonus, statModType.Flat, this)); 
        if(StrengthBonus != 0) 
            c.Strength.AddModifier(new StatModifier(StrengthBonus, statModType.Flat, this));
        if(DexterityBonus != 0) 
            c.Dexterity.AddModifier(new StatModifier(DexterityBonus, statModType.Flat, this));
        if(CharismaBonus != 0) 
            c.Charisma.AddModifier(new StatModifier(CharismaBonus, statModType.Flat, this));
        if(WeaponArmourBonus != 0) 
            c.WeaponArmour.AddModifier(new StatModifier(WeaponArmourBonus, statModType.Flat, this));
        if(MagicArmourBonus != 0) 
            c.MagicArmour.AddModifier(new StatModifier(MagicArmourBonus, statModType.Flat, this));
        if(ProjectileArmourBonus != 0) 
            c.ProjectileArmour.AddModifier(new StatModifier(ProjectileArmourBonus, statModType.Flat, this));
        if(ArmourPiercingBonus != 0) 
            c.ArmourPiercing.AddModifier(new StatModifier(ArmourPiercingBonus, statModType.Flat, this));



        if(MaxHealthPercentBonus != 0) 
            c.MaxHealth.AddModifier(new StatModifier(MaxHealthPercentBonus, statModType.PercentAdd, this)); 
        if(MaxManaPercentBonus != 0) 
            c.MaxMana.AddModifier(new StatModifier(MaxManaPercentBonus, statModType.PercentAdd, this)); 
        if(StrengthPercentBonus != 0) 
            c.Strength.AddModifier(new StatModifier(StrengthPercentBonus, statModType.PercentAdd, this));
        if(DexterityPercentBonus != 0) 
            c.Dexterity.AddModifier(new StatModifier(DexterityPercentBonus, statModType.PercentAdd, this));
        if(CharismaPercentBonus != 0) 
            c.Charisma.AddModifier(new StatModifier(CharismaPercentBonus, statModType.PercentAdd, this));
        if(WeaponArmourPercentBonus != 0) 
            c.WeaponArmour.AddModifier(new StatModifier(WeaponArmourPercentBonus, statModType.PercentAdd, this));
        if(MagicArmourPercentBonus != 0) 
            c.MagicArmour.AddModifier(new StatModifier(MagicArmourPercentBonus, statModType.PercentAdd, this));
        if(ProjectileArmourPercentBonus != 0) 
            c.ProjectileArmour.AddModifier(new StatModifier(ProjectileArmourPercentBonus, statModType.PercentAdd, this));
        if(ArmourPiercingPercentBonus != 0) 
            c.ArmourPiercing.AddModifier(new StatModifier(ArmourPiercingPercentBonus, statModType.PercentAdd, this));
    }

    public void Unequip(PlayerCharacter c)
    {
        c.MaxHealth.RemoveAllModifiersFromSource(this);
        c.MaxMana.RemoveAllModifiersFromSource(this);
        c.Strength.RemoveAllModifiersFromSource(this);
        c.Dexterity.RemoveAllModifiersFromSource(this);
        c.Charisma.RemoveAllModifiersFromSource(this);
        c.WeaponArmour.RemoveAllModifiersFromSource(this);
        c.MagicArmour.RemoveAllModifiersFromSource(this);
        c.ProjectileArmour.RemoveAllModifiersFromSource(this);
        c.ArmourPiercing.RemoveAllModifiersFromSource(this);
    }

    public override string GetItemType()
    {
        return EquipmentType.ToString();
    }

    public override string GetDescription()
    {
        sb.Length = 0;
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

    protected void AddStats(float value, string statName, bool isPercent = false) 
    {
        if(value != 0)
        {
            if(sb.Length > 0) sb.AppendLine();
            if(value > 0) sb.Append("+");
            if(isPercent)
            {
                sb.Append(value * 100);
                sb.Append("% ");
            }
            else
            {
                sb.Append(value);
                sb.Append(" ");
            }
            
            sb.Append(statName);
        }

    }
}


