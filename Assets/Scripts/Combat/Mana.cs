using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Mana : MonoBehaviour
    {
        [SerializeField] private float maxMana = 100;
        [SerializeField] private Character character;
        private float mana;
        void Start()
        {
            character.OnMaxManaUpdate += UpdateMaxMana;
            maxMana = character.MaxMana.Value;
            mana = maxMana;
        }

        private void UpdateMaxMana()
        {
            mana += character.MaxMana.Value - maxMana;
            maxMana = character.MaxMana.Value;
        }

        public bool ReduceMana(float amount)
        {
            if(mana - amount < 0)
            {
                return false;
            }
            else
            {
                mana -= amount;
                return true;
            }
        }

        public void IncreasMana(float amount)
        {
            mana = Mathf.Min(mana + amount, maxMana);
        }

        public float GetMana()
        {
            return mana;
        }

        public float GetPercentageOfMana()
        {
            return mana / maxMana;
        }

    }
}

