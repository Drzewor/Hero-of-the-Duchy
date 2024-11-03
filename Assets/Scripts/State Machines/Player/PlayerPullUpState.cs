using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerPullUpState : PlayerBaseState
    {
        private readonly int PullUpAnimationtHash = Animator.StringToHash("Pullup");
        private const float CrossFadeDuration = 0.1f;
        private readonly Vector3 offSet = new Vector3(0f,1.2f,0.9f);
        public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(PullUpAnimationtHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            if(GetNormalizedTime(stateMachine.Animator,"Climbing") <= 1f) return;

            stateMachine.characterController.enabled = false;

            stateMachine.transform.Translate(offSet, Space.Self);

            stateMachine.characterController.enabled = true;

            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine, false));
        }

        public override void Exit()
        {
            stateMachine.characterController.Move(Vector3.zero);
            stateMachine.ForceReceiver.Reste();
        }


    }

}
