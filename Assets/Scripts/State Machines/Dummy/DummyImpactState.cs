using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine;
using UnityEngine;

namespace RPG.StateMachine.Dummy
{
    public class DummyImpactState : State
    {
        private DummyStateMachine stateMachine;
        private readonly int ImpactAnimationHash = Animator.StringToHash("Impact");
        private const float CrossFadeDuration = 0.1f;
        private float duration = 0.66f;

        public DummyImpactState(DummyStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash,CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            duration -= deltaTime;
            if(duration <= 0)
            {
                stateMachine.SwitchState(new DummyIdleState(stateMachine));
            }
        }
        
        public override void Exit(){}
    }
}

