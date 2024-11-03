using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerAttackingState : PlayerBaseState
    {
        private float previousFrameTime;
        private bool alreadyAppliedForce = false;
        private Attack attack;
        public PlayerAttackingState(PlayerStateMachine stateMachine, int attackIndex) : base(stateMachine)
        {
            attack = stateMachine.Attacks[attackIndex];
        }
        

        public override void Enter()
        {
            stateMachine.WeaponLogic.SetAttack(stateMachine.AttackDamage, attack.Knockback, attack.DamageMultiplier);
            stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            FaceTarget();

            float normalizedTime = GetNormalizedTime(stateMachine.Animator, "Attack");
            if(normalizedTime >= previousFrameTime && normalizedTime < 1f)
            {
                if(attack.ForceTime <= normalizedTime)
                {
                    TryApplyforce();
                }

                if(stateMachine.InputReader.IsAttacking)
                {
                    TryComboAttack(normalizedTime);
                }
            }
            else
            {
                ReturnToLocomotion();
            }
            //TryApplyforce();
            previousFrameTime = normalizedTime;
        }

        public override void Exit()
        {
            
        }

        private void TryComboAttack(float normalizedTime)
        {
            if (attack.ComboStateIndex == -1) { return; }

            if (normalizedTime < attack.ComboAttackTime) { return; }

            if (!stateMachine.Stamina.ReduceStamina(stateMachine.Attacks[attack.ComboStateIndex].StaminaCost)) 
            { 
                return; 
            }

            stateMachine.SwitchState
            (
                new PlayerAttackingState
                (
                    stateMachine,
                    attack.ComboStateIndex
                )
            );
        }

        private void TryApplyforce()
        {
            if(alreadyAppliedForce) return;

            stateMachine.ForceReceiver.AddForce(stateMachine.transform.forward * attack.Force);

            alreadyAppliedForce = true;
        }
    }

}
