using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCChasingState : NPCBaseState
    {
        private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        //TO DO: Make Better DodgingDistance
        private const float DodgingDistance = 7;
        StateMachine targetStateMachine;
        private float circlingRange;
        private float dodgeRoll = 0;
        NPCBaseState previousState;
        public NPCChasingState(NPCStateMachine stateMachine, NPCBaseState previousState) : base(stateMachine)
        {
            this.previousState = previousState;
        }

        public override void Enter()
        {
            dodgeRoll = Random.Range(1,101);
            circlingRange = Random.Range(stateMachine.MinCircleRange,stateMachine.MaxCircleRange);

            if(stateMachine.NPCTargeter.currentTarget != null)
            {
                targetStateMachine = stateMachine.NPCTargeter.currentTarget.gameObject.GetComponent<StateMachine>();
            }
            
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if(stateMachine.NPCTargeter.currentTarget == null && stateMachine.TargetToFollow != null)
            {
                stateMachine.SwitchState(new NPCFollowingState(stateMachine));
                return;
            }
            if(stateMachine.NPCTargeter.currentTarget == null)
            {
                stateMachine.SwitchState(new NPCMovingState(stateMachine, stateMachine.NPCTargeter.lastTargetPosition, true));
                return;
            }
            else if(stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= stateMachine.AttackRange * stateMachine.AttackRange)
            {
                stateMachine.SwitchState(new NPCAttackingState(stateMachine,0));
                return;
            }
            else if(!(previousState is NPCCirclingState) && stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= circlingRange * circlingRange)
            {
                stateMachine.SwitchState(new NPCCirclingState(stateMachine));
                return;
            }
            else if
                (
                dodgeRoll >= 51 && 
                (targetStateMachine.GetCurrentState() is PlayerAttackingState || 
                targetStateMachine.GetCurrentState() is NPCAttackingState) &&
                stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= DodgingDistance
                )
            {
                stateMachine.SwitchState(new NPCDodgingState(stateMachine));
                return;
            }

            MoveToTarget(deltaTime, stateMachine.NPCTargeter.currentTarget.gameObject);

            stateMachine.Animator.SetFloat(SpeedHash,1, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.ResetPath();
            }
        }
        
        private void MoveToTarget(float deltaTime, GameObject target)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = target.transform.position;

                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);
                if(stateMachine.Agent.path.corners.Length > 1)
                {
                    FaceNextCorner();
                }
                else
                {
                    FaceTarget();
                }
            }
            
            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }
    }

}
