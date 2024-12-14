using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCIdleState : NPCBaseState
    {
        private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        public NPCIdleState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if(stateMachine.NPCTargeter.currentTarget != null && !stateMachine.NPCTargeter.currentTarget.isDead)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine, this));
                return;
            }
            if(stateMachine.TargetToFollow != null)
            {
                stateMachine.SwitchState(new NPCFollowingState(stateMachine));
                return;
            }
            if(GetSqrDistanceToPoint(stateMachine.HomePosition) > 2)
            {
                stateMachine.SwitchState(new NPCMovingState(stateMachine,stateMachine.HomePosition));
                return;
            }

            stateMachine.Animator.SetFloat(SpeedHash,0, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            
        }
    }
}

