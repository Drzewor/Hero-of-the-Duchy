using System.Collections;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.CharacterStats
{
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
}

