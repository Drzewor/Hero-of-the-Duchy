using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerFallingState : PlayerBaseState
    {
        private readonly int FallAnimationtHash = Animator.StringToHash("Fall");
        private const float CrossFadeDuration = 0.1f;
        private Vector3 momentum;
        public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            momentum = stateMachine.characterController.velocity;
            momentum.y = 0f;
            stateMachine.Animator.CrossFadeInFixedTime(FallAnimationtHash, CrossFadeDuration);

            stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum,deltaTime);

            if(stateMachine.characterController.isGrounded)
            {
                ReturnToLocomotion();
            }

            FaceTarget();
        }

        public override void Exit()
        {
            stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        }

        private void HandleLedgeDetect(Vector3 ledgeForward, Vector3 closestPoint)
        {
            stateMachine.SwitchState(new PlayerHangingState(stateMachine,ledgeForward, closestPoint));
        }
    }
}

