using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public abstract class PlayerBaseState : State
    {
        protected PlayerStateMachine stateMachine;
        private float YAxisSpeed;
        private float XAxisSpeed;
        private const float rotationSpeed = 5;

        public PlayerBaseState(PlayerStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        protected void Move(Vector3 motion, float deltaTime)
        {
            stateMachine.characterController.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
        }

        protected void Move(float deltaTime)
        {
            Move(Vector3.zero, deltaTime);
        }

        protected void FaceTarget()
        {
            if(stateMachine.Targeter.CurrentTarget == null) return;

            Vector3 lookPosition = 
            stateMachine.Targeter.CurrentTarget.transform.position - stateMachine.transform.position;
            lookPosition.y = 0f;
            Quaternion targetRotation = Quaternion.LookRotation(lookPosition);

            stateMachine.transform.rotation = Quaternion.Slerp(
                stateMachine.transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed);
        }

        protected void ReturnToLocomotion()
        {
            if(stateMachine.Targeter.CurrentTarget != null)
            {
                stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            }
            else
            {
                stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            }
        }

        protected void EnterUIMode()
        {
            YAxisSpeed = stateMachine.cinemachineFreeLook.m_YAxis.m_MaxSpeed;
            XAxisSpeed = stateMachine.cinemachineFreeLook.m_XAxis.m_MaxSpeed;
            stateMachine.cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0;
            stateMachine.cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;        
        }

        protected void ExitUIMode()
        {
            stateMachine.cinemachineFreeLook.m_YAxis.m_MaxSpeed = YAxisSpeed;
            stateMachine.cinemachineFreeLook.m_XAxis.m_MaxSpeed = XAxisSpeed;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

}
