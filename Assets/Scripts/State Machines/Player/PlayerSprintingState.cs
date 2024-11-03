using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.StateMachine.Player;
using UnityEngine;


namespace RPG.StateMachine.Player
{
    public class PlayerSprintingState : PlayerFreeLookState
    {
        public PlayerSprintingState(PlayerStateMachine stateMachine) : base(stateMachine){}
        private readonly int FreelookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int SprintBlendTreeHash = Animator.StringToHash("SprintBlendTree");
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;
        private const float staminaCost = 2f;
        private Attack firstAttack;

        public override void Enter()
        {
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.InputReader.InteractEvent += OnInteract;

            firstAttack = stateMachine.Attacks[0];

            stateMachine.Animator.CrossFadeInFixedTime(SprintBlendTreeHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if(stateMachine.InputReader.IsAttacking && stateMachine.Stamina.ReduceStamina(firstAttack.StaminaCost))
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
                return;
            }
            if(!stateMachine.InputReader.IsSprinting || !stateMachine.Stamina.ReduceStamina(staminaCost * deltaTime))
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }

            Vector3 movment = CalculateMovement();

            Move(movment * stateMachine.SprintMovmentSpeed, deltaTime);

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                return;
            }
            stateMachine.Animator.SetFloat(FreelookSpeedHash, 1, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movment, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.JumpEvent -= OnJump;
            stateMachine.InputReader.InteractEvent -= OnInteract;
        }

}

}
