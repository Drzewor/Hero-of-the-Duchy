using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New UsableItem", menuName = "RPG/New UsableItem", order = 2)]
public class UsableItem : Item
{
    public bool IsConsumable = true;
    public List<UsableItemEffect> Effects;
    public virtual void Use(InventoryManager character)
    {
        foreach(UsableItemEffect effect in Effects)
        {
            effect.ExecuteEffect(this, character);
        }
    }

    public override string GetItemType()
    {
        return IsConsumable ? "Consumable" : "Usable";
    }

    public override string GetDescription()
    {
        sb.Length = 0;

        foreach(UsableItemEffect effect in Effects)
        {
            sb.Append(effect.GetDescription());
        }

        return sb.ToString();
    }
}
