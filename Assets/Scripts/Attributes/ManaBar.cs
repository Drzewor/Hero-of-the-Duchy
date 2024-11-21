using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

public class ManaBar : MonoBehaviour
{
        [SerializeField] Mana mana;
        [SerializeField] RectTransform foreground;

        private void Update() 
        {
            UpdateManaBar();
        }

        private void UpdateManaBar()
        {
            foreground.localScale = new Vector3(mana.GetPercentageOfMana(), 1, 1);
        }
}
