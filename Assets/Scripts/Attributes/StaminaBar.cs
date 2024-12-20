using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;


namespace RPG.CharacterStats
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] Stamina stamina;
        [SerializeField] RectTransform foreground;
    

        private void Update() 
        {
            UpdateStaminaBar();
        }

        private void UpdateStaminaBar()
        {
            foreground.localScale = new Vector3(stamina.GetPercentageOfStamina(), 1, 1);
        }
    }
}

