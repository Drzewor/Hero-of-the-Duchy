using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private readonly int ShieldBlockAnimationHash = Animator.StringToHash("ShieldBlock");
        private const float CrossFadeDuration = 0.1f;
        public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {   
            stateMachine.Animator.CrossFadeInFixedTime(ShieldBlockAnimationHash, CrossFadeDuration);
            stateMachine.Health.SetBlock(true);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);
            if(!stateMachine.InputReader.IsBlocking)
            {
                if(stateMachine.Targeter.CurrentTarget == null)
                {
                    stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
                    return;
                }
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
                return;
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetBlock(false);
        }
    }

}
