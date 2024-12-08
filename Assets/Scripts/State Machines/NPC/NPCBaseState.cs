using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public abstract class NPCBaseState : State
    {
        protected NPCStateMachine stateMachine;
        private const float rotationSpeed = 5;

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

        protected void MoveToDestination(float deltaTime, Vector3 destination)
        {
            if(stateMachine.Agent.isOnNavMesh)
            {
                stateMachine.Agent.destination = destination;
                Move(stateMachine.Agent.desiredVelocity.normalized * stateMachine.MovementSpeed, deltaTime);

                if(stateMachine.Agent.path.corners.Length > 1)
                {
                    FaceNextCorner();
                }
                else
                {
                    FaceTarget(destination);
                }
            }

            stateMachine.Agent.velocity = stateMachine.Controller.velocity;
            stateMachine.Agent.nextPosition = stateMachine.transform.position;
        }

        protected void FaceTarget()
        {
            if(stateMachine.NPCTargeter.currentTarget == null) return;

            Vector3 lookPosition = 
            stateMachine.NPCTargeter.currentTarget.transform.position - stateMachine.transform.position;
            lookPosition.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        protected void FaceTarget(Vector3 targetPoint)
        {
            Vector3 lookPosition = targetPoint - stateMachine.transform.position;
            lookPosition.y = 0f;

            stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
        }

        protected void FaceNextCorner()
        {
            if(!(stateMachine.Agent.path.corners.Length > 1)) return;

            Vector3 lookPosition = stateMachine.Agent.path.corners[1] - stateMachine.transform.position;
            lookPosition.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookPosition);
            
            stateMachine.transform.rotation = Quaternion.Slerp(stateMachine.transform.rotation,targetRotation,Time.deltaTime * rotationSpeed);
        }

        protected float GetSqrDistanceToPoint(Vector3 point)
        {
            return (point - stateMachine.transform.position).sqrMagnitude;
        }

    }

}
