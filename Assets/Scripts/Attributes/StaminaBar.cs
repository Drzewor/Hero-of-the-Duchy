using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;


namespace RPG.UI
{
    public class StaminaBar : MonoBehaviour
    {
        [SerializeField] Stamina stamina;
        [SerializeField] GameObject bar;
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

