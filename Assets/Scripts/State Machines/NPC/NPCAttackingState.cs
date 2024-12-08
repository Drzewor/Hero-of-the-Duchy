using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCAttackingState : NPCBaseState
    {
        private readonly int AttackAnimationHash = Animator.StringToHash("SwordAttack1");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private Attack attack;
        public NPCAttackingState(NPCStateMachine stateMachine ,int attackIndex) : base(stateMachine)
        {
            attack = stateMachine.Attacks[attackIndex];
        }

        public override void Enter()
        {
            FaceTarget();

            stateMachine.WeaponLogic.SetAttack(
                stateMachine.AttackDamage, 
                stateMachine.StatDamageBonus, 
                attack.Knockback, 
                attack.DamageMultiplier,
                stateMachine.Character.ArmourPiercing.Value, 
                stateMachine.IsWeaponMagic);
            stateMachine.Animator.CrossFadeInFixedTime(attack.AnimationName, attack.TransitionDuration);
        }

        public override void Tick(float deltaTime)
        {
            
            if(GetNormalizedTime(stateMachine.Animator, "Attack") >= 1f)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine,this));
                return;
            }
            if(GetNormalizedTime(stateMachine.Animator, "Attack") >= attack.ComboAttackTime)
            {
                TryCombo();
            }
            
        }

        public override void Exit()
        {
            
        }

        private void TryCombo()
        {
            if (attack.ComboStateIndex == -1) { return; }
            if (stateMachine.NPCTargeter.GetDistanceToTargetSqr() > stateMachine.AttackRange+1) return;
            stateMachine.SwitchState(new NPCAttackingState(stateMachine,attack.ComboStateIndex));
        }

    }

}
