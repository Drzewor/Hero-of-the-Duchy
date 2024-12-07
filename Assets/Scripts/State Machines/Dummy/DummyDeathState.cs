using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine;
using RPG.StateMachine.Dummy;
using UnityEngine;

namespace RPG.StateMachine.Dummy
{
    public class DummyDeathState : State
    {
        private DummyStateMachine stateMachine;
        private readonly int DeadAnimationHash = Animator.StringToHash("Dead");
        private const float CrossFadeDuration = 0.1f;
        public DummyDeathState(DummyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(DeadAnimationHash,CrossFadeDuration);
            stateMachine.Target.enabled = false;
            stateMachine.Controller.enabled = false;
        }

        public override void Tick(float deltaTime){}

        public override void Exit(){}
    }
}

