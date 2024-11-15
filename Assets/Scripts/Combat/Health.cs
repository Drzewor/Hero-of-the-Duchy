using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RPG.CharacterStats;
using RPG.Saving;
using RPG.StateMachine.NPC;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.Combat
{
    public class Health : MonoBehaviour, ISaveable
    {
        [SerializeField] private float maxHealth = 100;
        //private float HealthThreshold;
        private float healthPoints;
        public event Action OnTakeDamage;
        public event Action OnDie;
        public bool isDead = false;
        private bool isBlocking = false;
        private bool isDodging = false;
        [SerializeField] Character character;
        
        private void Start() 
        {
            character.OnMaxHealthUpdate += UpdateMaxHealth;
            maxHealth = character.MaxHealth.Value;
            healthPoints = maxHealth;
        }

        private void UpdateMaxHealth()
        {
            healthPoints += character.MaxHealth.Value - maxHealth;
            maxHealth = character.MaxHealth.Value;
        }

        public void SetBlock(bool isBlocking)
        {
            this.isBlocking = isBlocking;
        }

        public void SetDodge(bool isDodging)
        {
            this.isDodging = isDodging;
        }

        public void DealDamage(float damage, GameObject dealer, float armourPiercing = 0, bool isMagic = false)
        {
            if(healthPoints == 0) return;
            if(isDodging) return;

            if(isBlocking)
            {
                Vector3 direction = dealer.transform.position - transform.position;
                if(Vector3.Angle(transform.forward, direction)<40)
                {
                    return;
                } 
            }

            if(isMagic)
            {
                damage = Mathf.Max(damage-Mathf.Max(character.MagicArmour.Value - armourPiercing,0),0);
                if(damage <= 0)
                {
                    return;
                }
            }
            else
            {
                damage = Mathf.Max(damage-Mathf.Max(character.WeaponArmour.Value - armourPiercing,0),5);
            }

            healthPoints = Mathf.Max(healthPoints-damage, 0f);

            if(healthPoints <= 0 && !isDead)
            {
                isDead = true;
                OnDie?.Invoke();
            }
            else
            {
                GetComponent<NPCStateMachine>()?.NPCTargeter.SetLastKnowPosition(dealer.transform.position);
                OnTakeDamage?.Invoke();
            }
            
        }

        public void Heal(float healthAmount)
        {
            healthPoints = Mathf.Min(healthPoints + healthAmount, maxHealth);
        }

        public float GetPercentageOfHealth()
        {
            return healthPoints / maxHealth;
        }

        public float GeHealthPoints()
        {
            return healthPoints;
        }

        public object CaptureState()
        {
            return healthPoints;
        }

        public void RestoreState(object state)
        {
            healthPoints = (float)state;
            if(healthPoints <= 0)
            {
                isDead = true;
            }
            else
            {
                isDead = false;
            }

            if(TryGetComponent(out NPCStateMachine npcStateMachine))
            {
                if(isDead)
                {
                    npcStateMachine.SwitchState(new NPCDeadState(npcStateMachine));
                    return;
                }
                else
                {
                    npcStateMachine.SwitchState(new NPCIdleState(npcStateMachine));
                    return;
                }

            }
            if(TryGetComponent(out PlayerStateMachine stateMachine))
            {
                if(isDead)
                {
                    stateMachine.SwitchState(new PlayerDeadState(stateMachine));
                    return;
                }
                else
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                    return;
                }
            }

        }
    }

}
