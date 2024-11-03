using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerDodgingState : PlayerBaseState
    {
        
        private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
        private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
        private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
        private const float CrossFadeDuration = 0.1f;
        private float remainingDodgeTime;
        private Vector3 dodgingDirectionInput;
        public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine)
        {
            this.dodgingDirectionInput = dodgingDirectionInput;
        }

        public override void Enter()
        {
            remainingDodgeTime = stateMachine.DodgeDuration;

            stateMachine.Animator.SetFloat(DodgeForwardHash, stateMachine.InputReader.MovementValue.y);
            stateMachine.Animator.SetFloat(DodgeRightHash, stateMachine.InputReader.MovementValue.x);
            stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash,CrossFadeDuration);

            stateMachine.Health.SetDodge(true);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();
            
            movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

            Move(movement, deltaTime);

            FaceTarget();

            remainingDodgeTime -= deltaTime;

            if(remainingDodgeTime <= 0)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            
        }

        public override void Exit()
        {
            stateMachine.Health.SetDodge(false);
        }


    }

}
