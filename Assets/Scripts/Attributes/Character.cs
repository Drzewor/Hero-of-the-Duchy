using System;
using System.Collections;
using System.Collections.Generic;
using RPG.CharacterStats;
using RPG.Saving;
using UnityEngine;

public class Character : MonoBehaviour, ISaveable
{
    [SerializeField] public CharacterStat MaxHealth;
    [SerializeField] public CharacterStat MaxMana;
    [SerializeField] public CharacterStat Strength;
    [SerializeField] public CharacterStat Dexterity;
    [SerializeField] public CharacterStat Charisma;
    [SerializeField] public CharacterStat WeaponArmour;
    [SerializeField] public CharacterStat MagicArmour;
    [SerializeField] public CharacterStat ArmourPiercing;
    [Space]
    [SerializeField] private CharacterExperience characterExperience;
    [SerializeField] private StatPanel statPanel;
    public event Action OnMaxHealthUpdate;

    //Called On StatIncreaseButtons
    public void IncreaseAttribute(string stat)
    {
        if(!characterExperience.SpendLerningPoints(1)) return;
        switch (stat)
        {
            case "MaxHealth":
                MaxHealth.BaseValue += 10;
                statPanel?.UpdateStatValues();
                OnMaxHealthUpdate?.Invoke();
                return;
            case "MaxMana":
                MaxMana.BaseValue += 10;
                statPanel?.UpdateStatValues();
                return;
            case "Strength":
                Strength.BaseValue += 1;
                statPanel?.UpdateStatValues();
                return;
            case "Dexterity":
                Dexterity.BaseValue += 1;
                statPanel?.UpdateStatValues();
                return;
            case "Charisma":
                Charisma.BaseValue += 1;
                statPanel?.UpdateStatValues();
                return;
            default:
            return;
        }
    }

    public object CaptureState()
    {
        List<float> saveData = new List<float>();
        saveData.Add(MaxHealth.BaseValue);
        saveData.Add(MaxMana.BaseValue);
        saveData.Add(Strength.BaseValue);
        saveData.Add(Dexterity.BaseValue);
        saveData.Add(Charisma.BaseValue);

        return saveData;
    }

    public void RestoreState(object state)
    {
        List<float> saveData = (List<float>)state;
        MaxHealth.BaseValue = saveData[0];
        MaxMana.BaseValue = saveData[1];
        Strength.BaseValue = saveData[2];
        Dexterity.BaseValue = saveData[3];
        Charisma.BaseValue = saveData[4];
        if(statPanel != null)
        {
            statPanel.UpdateStatValues();
        }
    }
}
