using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

[CreateAssetMenu(fileName = "New HealItemEffect", menuName = "RPG/Item Effects/Heal", order = 0)]
public class HealItemEffect : UsableItemEffect
{
    public float HealthAmount = 10;
    public override void ExecuteEffect(UsableItem parentItem, PlayerCharacter character)
    {
        character.GetComponent<Health>().Heal(HealthAmount);
    }

    public override string GetDescription()
    {
        return $"Heals for {HealthAmount} health";
    }
}
