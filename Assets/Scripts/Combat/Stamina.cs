using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] float maxStamina = 100;
        [SerializeField] float regenSpeed = 5;
        [SerializeField] float RegenCooldown = 2;
        float timeSinceReduced = Mathf.Infinity;
        bool isRecentlyReduced = false;
        private float stamina;
        
        private void Awake() 
        {
            stamina = maxStamina;
        }

        private void Update() 
        {
            float deltaTime = Time.deltaTime;

            if(isRecentlyReduced)
            {
                CountCooldown(deltaTime);
            }
            else
            {
                RegenerateStamina(deltaTime);
            }
        }

        private void CountCooldown(float deltaTime)
        {
            timeSinceReduced += deltaTime;
            if(timeSinceReduced >= RegenCooldown)
            {
                isRecentlyReduced = false;
            }
        }

        private void RegenerateStamina(float deltaTime)
        {
            if(stamina < maxStamina)
            {
                stamina += regenSpeed * deltaTime;
            }
        }

        public bool ReduceStamina(float amount)
        {
            if(stamina - amount < 0)
            {
                return false;
            }
            else
            {
                timeSinceReduced = 0;
                isRecentlyReduced = true;
                stamina -= amount;
                return true;
            }
        }

        public float GetPercentageOfStamina()
        {
            return stamina / maxStamina;
        }
    }
}
