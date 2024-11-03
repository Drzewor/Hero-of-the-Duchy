using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCDeadState : NPCBaseState
    {
        private readonly int DeathAnimationHash = Animator.StringToHash("Death");
        private const float CrossFadeDuration = 0.1f;
        private bool isWeaponOff = false;
        public NPCDeadState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Target.enabled = false;
            stateMachine.Animator.CrossFadeInFixedTime(DeathAnimationHash, CrossFadeDuration);
            stateMachine.Controller.enabled = false;
        }

        public override void Tick(float deltaTime)
        {
            if(!isWeaponOff && stateMachine.WeaponLogic != null)
            {
                isWeaponOff = true;
                stateMachine.WeaponLogic.gameObject.SetActive(false);
            }
        }

        public override void Exit()
        {
        }


    }
}

