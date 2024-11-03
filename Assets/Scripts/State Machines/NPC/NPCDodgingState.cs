using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCDodgingState : NPCBaseState
    {
        private readonly int DodgeBlendTreeHash = Animator.StringToHash("DodgeBlendTree");
        private readonly int DodgeForwardHash = Animator.StringToHash("DodgeForward");
        private readonly int DodgeRightHash = Animator.StringToHash("DodgeRight");
        private const float CrossFadeDuration = 0.1f;
        private float remainingDodgeTime;
        private int dodgingDirectionRoll;
        private Vector3 dodgingDirection;
        public NPCDodgingState(NPCStateMachine stateMachine) : base(stateMachine)
        {}

        public override void Enter()
        {
            remainingDodgeTime = stateMachine.DodgeDuration;
            dodgingDirectionRoll = Random.Range(-1,2);
            if(dodgingDirectionRoll == -1)
            {
                stateMachine.Animator.SetFloat(DodgeForwardHash, 0);
                stateMachine.Animator.SetFloat(DodgeRightHash, -1);
                dodgingDirection.y = 0;
                dodgingDirection.x = -1;
            }
            else if(dodgingDirectionRoll == 0)
            {
                stateMachine.Animator.SetFloat(DodgeForwardHash, -1);
                stateMachine.Animator.SetFloat(DodgeRightHash, 0);    
                dodgingDirection.y = -1;
                dodgingDirection.x = 0;            
            }
            else if(dodgingDirectionRoll == 1)
            {
                stateMachine.Animator.SetFloat(DodgeForwardHash, 0);
                stateMachine.Animator.SetFloat(DodgeRightHash, 1); 
                dodgingDirection.y = 0;
                dodgingDirection.x = 1;               
            }

            stateMachine.Animator.CrossFadeInFixedTime(DodgeBlendTreeHash,CrossFadeDuration);

            stateMachine.Health.SetDodge(true);
        }

        public override void Tick(float deltaTime)
        {
            Vector3 movement = new Vector3();
            
            movement += stateMachine.transform.right * dodgingDirection.x * stateMachine.DodgeDistance / stateMachine.DodgeDuration;
            movement += stateMachine.transform.forward * dodgingDirection.y * stateMachine.DodgeDistance / stateMachine.DodgeDuration;

            Move(movement, deltaTime);

            //FaceTarget();            

            remainingDodgeTime -= deltaTime;

            if(remainingDodgeTime <= 0)
            {
                stateMachine.SwitchState(new NPCChasingState(stateMachine));
            }
        }

        public override void Exit()
        {
            stateMachine.Health.SetDodge(false);
        }


    }
}

