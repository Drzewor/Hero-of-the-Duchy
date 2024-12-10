using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCImpactState : NPCBaseState
    {
        private readonly int ImpactAnimationHash = Animator.StringToHash("Impact");
        private const float CrossFadeDuration = 0.1f;
        private float duration = 1;
        public NPCImpactState(NPCStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(ImpactAnimationHash, CrossFadeDuration);
            stateMachine.WeaponLogic.gameObject.GetComponent<Collider>().enabled = false;
            stateMachine.WeaponLogic.ClearColidersList();
            stateMachine.WeaponLogic.SwitchTrails(false);
        }

        public override void Tick(float deltaTime)
        {
            Move(deltaTime);

            duration -= deltaTime;

            if(duration <= 0f)
            {
                    stateMachine.SwitchState(new NPCChasingState(stateMachine, this));
            }
        }

        public override void Exit()
        {
        
        }


    }
}

