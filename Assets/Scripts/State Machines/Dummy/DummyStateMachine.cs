using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.StateMachine;
using UnityEngine;

namespace RPG.StateMachine.Dummy
{
    public class DummyStateMachine : StateMachine
    {
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public Target Target {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        private void OnEnable() 
        {
            Health.OnTakeDamage += HandleTakeDamange;
            Health.OnDie += HandleDeath;
            SwitchState(new DummyIdleState(this));
        }

        private void HandleTakeDamange()
        {
            SwitchState(new DummyImpactState(this));
        }
        
        private void HandleDeath(Health health)
        {
            SwitchState(new DummyDeathState(this));
        }
    }
}

