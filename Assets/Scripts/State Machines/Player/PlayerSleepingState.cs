using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;
using UnityEngine.Experimental.AI;

namespace RPG.StateMachine.Player
{
    public class PlayerSleepingState : PlayerBaseState
    {
        private readonly int SleepingAnimationtHash = Animator.StringToHash("Sleep");
        private const float CrossFadeDuration = 0.1f;
        private Transform snapingPosition;
        private Bed bed;

        public PlayerSleepingState(PlayerStateMachine stateMachine, Transform snapingPosition, Bed bed) : base(stateMachine)
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
            if(stateMachine.InputReader.MovementValue.y != 0f)
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine,false));
            }
        }

        public override void Exit()
        {
            bed.SetisOccupied(false);
        }

    }
}

