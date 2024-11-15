using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public abstract class NPCBaseState : State
    {
        protected NPCStateMachine stateMachine;
        public NPCBaseState(NPCStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void FaceTarget()
        {
            if(stateMachine.NPCTargeter.target == null) return;

            Vector3 lookPosition = 
            stateMachine.NPCTargeter.target.transform.position - stateMachine.transform.position;
            lookPosition.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        protected void FaceTarget(Vector3 targetPoint)
        {
            Vector3 lookPosition = targetPoint - stateMachine.transform.position;
            lookPosition.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        protected float GetSqrDistanceToPoint(Vector3 point)
        {
            return (point - stateMachine.transform.position).sqrMagnitude;
        }

    }

}
