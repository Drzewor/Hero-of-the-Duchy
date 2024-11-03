using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerJumpingState : PlayerBaseState
    {
        private readonly int JumpAnimationtHash = Animator.StringToHash("Jump");
        private const float CrossFadeDuration = 0.1f;
        private Vector3 momentum;
        public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

            momentum = stateMachine.characterController.velocity;
            momentum.y = 0f;

            stateMachine.Animator.CrossFadeInFixedTime(JumpAnimationtHash, CrossFadeDuration);

            //stateMachine.LedgeDetector.OnLedgeDetect += HandleLedgeDetect;
        }

        public override void Tick(float deltaTime)
        {
            Move(momentum,deltaTime);

            if(stateMachine.characterController.velocity.y <= 0)
            {
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
                return;
            }

            FaceTarget();
        }

        public override void Exit()
        {
            //stateMachine.LedgeDetector.OnLedgeDetect -= HandleLedgeDetect;
        }

        /*private void HandleLedgeDetect(Vector3 ledgeForward)
        {
            stateMachine.SwitchState(new PlayerHangingState(stateMachine,ledgeForward));
        }*/

    }

}
