using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerImpactState : PlayerBaseState
    {
        private readonly int ImpactAnimationHash = Animator.StringToHash("Impact");
        private const float CrossFadeDuration = 0.1f;
        private float duration = 1;
        public PlayerImpactState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            duration -= deltaTime;
            if(duration <= 0)
            {
                ReturnToLocomotion();
            }

        }

        public override void Exit()
        {
    
        }


    }
}

