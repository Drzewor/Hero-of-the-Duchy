using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Dummy
{
    public class DummyIdleState : State
    {
        private DummyStateMachine stateMachine;
        private readonly int IdleAnimationHash = Animator.StringToHash("Idle");
        private const float CrossFadeDuration = 0.1f;
        public DummyIdleState(DummyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime){}

        public override void Exit(){}
    }

}
