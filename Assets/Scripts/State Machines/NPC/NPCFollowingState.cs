using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCFollowingState : NPCBaseState
    {
        private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private Transform targetToFollow;
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private const float MinDistanceToTarget = 5.5f;

        public NPCFollowingState(NPCStateMachine stateMachine, Transform targetToFollow = null) : base(stateMachine)
        {
            if(targetToFollow != null)
            {
                stateMachine.SetTargetToFollow(targetToFollow);
                this.targetToFollow = targetToFollow;
            }
            else
            {
                this.targetToFollow = stateMachine.TargetToFollow;
            }
            
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);  
        }

        public override void Tick(float deltaTime)
        {
            if(stateMachine.NPCTargeter.currentTarget != null && !stateMachine.NPCTargeter.currentTarget.isDead)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine,this));
                return;
            }

            if(GetSqrDistanceToPoint(targetToFollow.position) <= MinDistanceToTarget)
            {
                stateMachine.Animator.SetFloat(SpeedHash,0, AnimatorDampTime, deltaTime);
                return;
            }

            MoveToDestination(deltaTime ,targetToFollow.position);
            stateMachine.Animator.SetFloat(SpeedHash,1, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }
        }


    }
}

