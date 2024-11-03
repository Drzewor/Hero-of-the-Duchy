using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] CharacterAttributes characterAttr;
        
        private void Awake() 
        {
            healthPoints = maxHealth;
            return;
        }

        public void SetBlock(bool isBlocking)
        {
            this.isBlocking = isBlocking;
        }

        public void SetDodge(bool isDodging)
        {
            this.isDodging = isDodging;
        }

        public void DealDamage(float damage, GameObject dealer)
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
