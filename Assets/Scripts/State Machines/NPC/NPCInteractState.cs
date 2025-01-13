using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCInteractState : NPCBaseState
    {
        private readonly int SpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int LocomotionBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private const float CrossFadeDuration = 0.1f;
        private const float AnimatorDampTime = 0.1f;
        private const float DistanceToInteract = 3f;
        private IInteractable objectToInteract;
        private Vector3 objectLocation;
        public NPCInteractState(NPCStateMachine stateMachine, IInteractable objectToInteract, Vector3 objectLocation) : base(stateMachine)
        {
            this.objectToInteract = objectToInteract;
            this.objectLocation = objectLocation;
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
                stateMachine.SwitchState(new NPCChasingState(stateMachine, this));
                return;
            }        
            if(GetSqrDistanceToPoint(objectLocation) <= DistanceToInteract * DistanceToInteract)
            {
                objectToInteract.Interaction(stateMachine.gameObject);
                return;
            }
            MoveToDestination(deltaTime ,objectLocation);

            stateMachine.Animator.SetFloat(SpeedHash,1, AnimatorDampTime, deltaTime);
        }

        public override void Exit()
        {
            
        }
    }
}

