using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using RPG.Combat;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerTargetingState : PlayerBaseState
    {
        private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
        private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
        private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;
        private const float DodgeCost = 10;
        private Attack firstAttack;

        public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine){}
        
        public override void Enter()
        {
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.DodgeEvent += OnDodge;
            stateMachine.InputReader.JumpEvent += OnJump;
            
            firstAttack = stateMachine.Attacks[0];

            stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash, CrossFadeDuration);        
        }

        public override void Tick(float deltaTime)
        {
            if(
                stateMachine.InputReader.IsAttacking && 
                stateMachine.Stamina.ReduceStamina(firstAttack.StaminaCost) && 
                stateMachine.Mana.ReduceMana(firstAttack.ManaCost))
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
                return;
            }
            if(stateMachine.InputReader.IsBlocking)
            {
                stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            }
            if(stateMachine.Targeter.CurrentTarget == null)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            Vector3 movement = CalculateMovement(deltaTime);
            Move(movement * stateMachine.TargetingMovmentSpeed, deltaTime);

            UpdateAnimator(deltaTime);

            FaceTarget();

        }

        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.DodgeEvent -= OnDodge;
            stateMachine.InputReader.JumpEvent -= OnJump;
        }

        private void OnTarget()
        {
            stateMachine.Targeter.Cancel();

            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        private Vector3 CalculateMovement(float deltaTime)
        {
            Vector3 movement = new Vector3();
        
            movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
            movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;
            
            return movement;
        }
        
        private void UpdateAnimator(float deltaTime)
        {
            if(stateMachine.InputReader.MovementValue.y == 0)
            {
                stateMachine.Animator.SetFloat(TargetingForwardHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputReader.MovementValue.y >0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(TargetingForwardHash, value, AnimatorDampTime, deltaTime);
            }

            if(stateMachine.InputReader.MovementValue.x == 0)
            {
                stateMachine.Animator.SetFloat(TargetingRightHash, 0, AnimatorDampTime, deltaTime);
            }
            else
            {
                float value = stateMachine.InputReader.MovementValue.x >0 ? 1f : -1f;
                stateMachine.Animator.SetFloat(TargetingRightHash, value, AnimatorDampTime, deltaTime);
            }
        }

        
        private void OnDodge()
        {
            if(stateMachine.InputReader.MovementValue == Vector2.zero) return;

            if(stateMachine.Stamina.ReduceStamina(DodgeCost))
            {
                stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
            }
        }

        private void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }
    }

}
