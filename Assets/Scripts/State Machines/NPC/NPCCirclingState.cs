using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCCirclingState : NPCBaseState
    {
        private readonly int RightSpeedHash = Animator.StringToHash("TargetingRight");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private int direction;
        private float circlingTime;
        public NPCCirclingState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.SetMovementSpeed(stateMachine.CirclingSpeed);
            direction = Random.Range(0,2);
            circlingTime = Random.Range(stateMachine.MinCirclingTime,stateMachine.MaxCirclingTime);
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            float distanceToTarget = stateMachine.NPCTargeter.GetDistanceToTargetSqr();

            if(circlingTime <= 0 || distanceToTarget <= stateMachine.ChargeRange * stateMachine.ChargeRange)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine, this));
                return;
            }
            else if(stateMachine.NPCTargeter.currentTarget == null || stateMachine.NPCTargeter.currentTarget.isDead)
            {
                stateMachine.SwitchState(new NPCMovingState(stateMachine, stateMachine.NPCTargeter.lastTargetPosition, true));
                return;
            }
            else if(distanceToTarget <= stateMachine.AttackRange * stateMachine.AttackRange)
            {
                stateMachine.SwitchState(new NPCAttackingState(stateMachine,0));
                return;
            }

            MoveSideWay(deltaTime);
            FaceTarget();
            circlingTime -= deltaTime;

            
        }

        public override void Exit()
        {
            stateMachine.SetMovementSpeed(stateMachine.RunningSpeed);
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }
        }

        private void MoveSideWay(float deltaTime)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                if(direction == 1)
                {
                    stateMachine.Agent.destination = stateMachine.gameObject.transform.position + stateMachine.gameObject.transform.right;
                    stateMachine.Animator.SetFloat(RightSpeedHash,1, AnimatorDampTime, deltaTime);
                }
                else
                {
                    stateMachine.Agent.destination = stateMachine.gameObject.transform.position + stateMachine.gameObject.transform.right * -1;
                    stateMachine.Animator.SetFloat(RightSpeedHash,-1, AnimatorDampTime, deltaTime);
                }
                

                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }
    }
}
