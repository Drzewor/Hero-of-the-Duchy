using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCMovingState : NPCBaseState
    {
        private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private Vector3 destination;
        private bool IsChasing;
        public NPCMovingState(NPCStateMachine stateMachine, Vector3 destination, bool IsChasing = false) : base(stateMachine)
        {
            this.destination = destination;
            this.IsChasing = IsChasing;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);        
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if(stateMachine.NPCTargeter.currentTarget != null && !stateMachine.NPCTargeter.currentTarget.isDead)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine));
                return;
            }
            if(GetSqrDistanceToPoint(destination) <= 2)
            {
                if(IsChasing)
                {
                    stateMachine.SwitchState(new NPCsuspiciousState(stateMachine));
                    return;
                }
                else
                {
                    stateMachine.SwitchState(new NPCIdleState(stateMachine));
                    return;
                }
                
            }

            MoveToDestination(deltaTime ,destination);

            stateMachine.Animator.SetFloat(SpeedHash,1, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }
        }

        
        private void MoveToDestination(float deltaTime, Vector3 destination)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = destination;
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

                if(stateMachine.Agent.path.corners.Length > 1)
                {
                    FaceNextCorner();
                }
                else
                {
                    FaceTarget(destination);
                }
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }
    }
}
