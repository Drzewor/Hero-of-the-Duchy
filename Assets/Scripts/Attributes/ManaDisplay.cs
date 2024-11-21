using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using TMPro;
using UnityEngine;

public class ManaDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text manaDisplay;
    [SerializeField] Mana mana;
    [SerializeField] Character character;

    private void Start() 
    {
        character.OnMaxManaUpdate += OnEnable;
    }

    private void OnEnable() 
    {
        manaDisplay.text = $"{mana.GetMana()}/{character.MaxMana.Value}";
    }
}
