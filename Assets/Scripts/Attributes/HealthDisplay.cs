using System.Collections;
using RPG.Combat;
using TMPro;
using UnityEngine;

namespace RPG.CharacterStats
{
    public class HealthDisplay : MonoBehaviour
    {
        [SerializeField] TMP_Text healthDisplay;
        [SerializeField] Health health;
        [SerializeField] Character character;

        private void Start() 
        {
            character.OnMaxHealthUpdate += OnEnable;
        }

        private void OnEnable() 
        {
            healthDisplay.text = $"{health.GeHealthPoints()}/{character.MaxHealth.Value}";
        }
    }
}

