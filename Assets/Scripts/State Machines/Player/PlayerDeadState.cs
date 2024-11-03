using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        private readonly int DeathAnimationHash = Animator.StringToHash("Death");
        private const float CrossFadeDuration = 0.1f;
        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.WeaponLogic.gameObject.SetActive(false);
            stateMachine.Animator.CrossFadeInFixedTime(DeathAnimationHash, CrossFadeDuration);
        }

        public override void Tick(float deltaTime)
        {
            
        }

        public override void Exit()
        {
            
        }


    }

}
