using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerHangingState : PlayerBaseState
    {
        private Vector3 closestPoint;
        private Vector3 ledgeForward;
        private readonly int HangingAnimationtHash = Animator.StringToHash("Hanging");
        private const float CrossFadeDuration = 0.1f;
        public PlayerHangingState(PlayerStateMachine stateMachine, Vector3 ledgeForward,Vector3 closestPoint) : base(stateMachine)
        {
            this.closestPoint = closestPoint;
            this.ledgeForward = ledgeForward;
        }

        public override void Enter()
        {
            stateMachine.characterController.enabled = false;
            
            stateMachine.transform.rotation = quaternion.LookRotation(ledgeForward, Vector3.up);
            stateMachine.transform.position 
            = closestPoint - (stateMachine.LedgeDetector.transform.position -stateMachine.transform.position);

            stateMachine.characterController.enabled = true;

            stateMachine.Animator.CrossFadeInFixedTime(HangingAnimationtHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if(stateMachine.InputReader.MovementValue.y < 0f)
            {
                stateMachine.characterController.Move(Vector3.zero);
                stateMachine.ForceReceiver.Reste();
                stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            }
            if(stateMachine.InputReader.MovementValue.y > 0f)
            {
                stateMachine.SwitchState(new PlayerPullUpState(stateMachine));
            }
        }

        public override void Exit()
        {
        }


    }

}
