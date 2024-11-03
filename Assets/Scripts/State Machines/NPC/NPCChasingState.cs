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
        bool isCharging;
        public NPCChasingState(NPCStateMachine stateMachine, bool isCharging = false) : base(stateMachine)
        {
            this.isCharging = isCharging;
        }

        public override void Enter()
        {
            dodgeRoll = Random.Range(1,101);
            circlingRange = Random.Range(stateMachine.MinCircleRange,stateMachine.MaxCircleRange);

            if(stateMachine.NPCTargeter.target != null)
            {
                targetStateMachine = stateMachine.NPCTargeter.target.gameObject.GetComponent<StateMachine>();
            }
            
            stateMachine.Animator.CrossFadeInFixedTime(LocomotionBlendTreeHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if(stateMachine.NPCTargeter.target != null && stateMachine.NPCTargeter.target.isDead)
            {
                stateMachine.SwitchState(new NPCsuspiciousState(stateMachine));
            }
            else if(stateMachine.NPCTargeter.target == null)
            {
                stateMachine.SwitchState(new NPCMovingState(stateMachine, stateMachine.NPCTargeter.lastTargetPosition, true));
                return;
            }
            else if(stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= stateMachine.AttackRange * stateMachine.AttackRange)
            {
                stateMachine.SwitchState(new NPCAttackingState(stateMachine,0));
                return;
            }
            else if(!isCharging && stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= circlingRange * circlingRange)
            {
                stateMachine.SwitchState(new NPCCirclingState(stateMachine));
                return;
            }
            else if
                (
                dodgeRoll >= 51 && 
                targetStateMachine.GetCurrentState() is PlayerAttackingState &&
                stateMachine.NPCTargeter.GetDistanceToTargetSqr() <= DodgingDistance
                )
            {
                stateMachine.SwitchState(new NPCDodgingState(stateMachine));
                return;
            }

            MoveToTarget(deltaTime, stateMachine.NPCTargeter.target.gameObject);
            FaceTarget();

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
            }
            

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }
    }

}
