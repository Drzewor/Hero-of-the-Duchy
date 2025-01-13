using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCSleepingState : NPCBaseState
    {
        private readonly int SleepingAnimationtHash = Animator.StringToHash("Sleep");
        private const float CrossFadeDuration = 0.1f;
        private Transform snapingPosition;
        private Bed bed;

        public NPCSleepingState(NPCStateMachine stateMachine, Transform snapingPosition, Bed bed) : base(stateMachine)
        {
            this.snapingPosition = snapingPosition;
            this.bed = bed;
        }

        public override void Enter()
        {
            stateMachine.transform.position = snapingPosition.position;
            stateMachine.transform.rotation = snapingPosition.rotation;
            
            stateMachine.Animator.CrossFadeInFixedTime(SleepingAnimationtHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {

        }

        public override void Exit()
        {
            bed.SetisOccupied(false);
            stateMachine.SetObjectToInteract(null);
        }
    }
}

