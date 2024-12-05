using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerBlockingState : PlayerBaseState
    {
        private readonly int ShieldBlockAnimationHash = Animator.StringToHash("ShieldBlock");
        private readonly int ParryAnimationHash = Animator.StringToHash("Parry");
        private const float CrossFadeDuration = 0.1f;
        private float remainingParryTime = 1;
        public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            if(stateMachine.HasShield)
            {
                stateMachine.Animator.CrossFadeInFixedTime(ShieldBlockAnimationHash, CrossFadeDuration);
                stateMachine.Health.SetBlock(true);
            }
            else
            {
                stateMachine.Animator.CrossFadeInFixedTime(ParryAnimationHash, CrossFadeDuration);
                stateMachine.Health.SetParrying(true);
                remainingParryTime = stateMachine.ParryTime;
            }
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            if(!stateMachine.HasShield)
            {
                remainingParryTime -= deltaTime;
            }

            if(!stateMachine.InputReader.IsBlocking || remainingParryTime <= 0)
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
            stateMachine.Health.SetParrying(false);
            stateMachine.SetPreviousParryTime(0);
        }
    }

}
