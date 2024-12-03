using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using Unity.Mathematics;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerFreeLookState : PlayerBaseState
    {
        private bool shouldFade;
        private readonly int FreelookSpeedHash = Animator.StringToHash("FreeLookSpeed");
        private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
        private const float AnimatorDampTime = 0.1f;
        private const float CrossFadeDuration = 0.1f;
        private float startSpeed = 0;
        private Attack firstAttack;

        public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true, float speed = 0) : base(stateMachine)
        {
            this.shouldFade = shouldFade;
            this.startSpeed = speed;
        }

        public override void Enter()
        {
            stateMachine.InputReader.TargetEvent += OnTarget;
            stateMachine.InputReader.JumpEvent += OnJump;
            stateMachine.InputReader.InteractEvent += OnInteract;
            stateMachine.InputReader.PressIEvent += HandleInventory;
            stateMachine.InputReader.PressJEvent += HandleQuestlog;

            stateMachine.Animator.SetFloat(FreelookSpeedHash,startSpeed);

            firstAttack = stateMachine.Attacks[0];

            if(shouldFade)
            {
                stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
            }
            else
            {
                stateMachine.Animator.Play(FreeLookBlendTreeHash);
            }
        }

        public override void Tick(float deltaTime)
        {
            if(stateMachine.InputReader.IsAttacking && stateMachine.Stamina.ReduceStamina(firstAttack.StaminaCost))
            {
                stateMachine.SwitchState(new PlayerAttackingState(stateMachine,0));
                return;
            }
            if(stateMachine.InputReader.IsSprinting && stateMachine.Stamina.GetPercentageOfStamina() >= 0.05)
            {
                stateMachine.SwitchState(new PlayerSprintingState(stateMachine));
                return;
            }
            if(stateMachine.InputReader.IsBlocking)
            {
                stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            }

            RaycastInteractable();

            Vector3 movment = CalculateMovement();

            Move(movment * stateMachine.FreeLookMovmentSpeed, deltaTime);

            if (stateMachine.InputReader.MovementValue == Vector2.zero)
            {
                stateMachine.Animator.SetFloat(FreelookSpeedHash, 0, AnimatorDampTime, deltaTime);
                return;
            }
            stateMachine.Animator.SetFloat(FreelookSpeedHash, 1, AnimatorDampTime, deltaTime);
            FaceMovementDirection(movment, deltaTime);
        }

        public override void Exit()
        {
            stateMachine.InputReader.TargetEvent -= OnTarget;
            stateMachine.InputReader.JumpEvent -= OnJump;
            stateMachine.InputReader.InteractEvent -= OnInteract;
            stateMachine.InputReader.PressIEvent -= HandleInventory;
            stateMachine.InputReader.PressJEvent -= HandleQuestlog;

            stateMachine.InteractionText.enabled = false;
        }

        protected void OnTarget()
        {
            if(!stateMachine.Targeter.SelectTarget()) return;

            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }

        protected Vector3 CalculateMovement()
        {
            Vector3 forward = stateMachine.MainCameraTransform.forward;
            Vector3 right = stateMachine.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
        }

        protected void FaceMovementDirection(Vector3 movment, float deltaTime)
        {
            stateMachine.transform.rotation = Quaternion.Lerp
            (stateMachine.transform.rotation,
            Quaternion.LookRotation(movment),
            deltaTime * stateMachine.RotationDamping) ;
        }

        protected void OnJump()
        {
            stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
        }

        protected void RaycastInteractable()
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                stateMachine.MainCameraTransform.position, stateMachine.InteractionRadius, 
                stateMachine.MainCameraTransform.forward, stateMachine.InteractionDistance
                );
            foreach(RaycastHit hit in hits)
            {
                IInteractable[] interactables = hit.transform.GetComponents<IInteractable>();
                foreach(IInteractable interactable in interactables)
                {
                    if(interactable !=null)
                    {
                        interactable.HandleRaycast(stateMachine.gameObject);
                        return;
                    }
                }
                stateMachine.InteractionText.enabled = false;
            }
        }

        protected void OnInteract()
        {
            RaycastHit[] hits = Physics.SphereCastAll(
                stateMachine.MainCameraTransform.position, stateMachine.InteractionRadius, 
                stateMachine.MainCameraTransform.forward, stateMachine.InteractionDistance
                );
            foreach(RaycastHit hit in hits)
            {
                IInteractable[] interactables = hit.transform.GetComponents<IInteractable>();
                foreach(IInteractable interactable in interactables)
                {
                    if(interactable !=null)
                    {
                        interactable.Interaction(stateMachine.gameObject);
                    }
                }
            }
        }

        protected void HandleInventory()
        {
            stateMachine.SwitchState(new PlayerInventoryState(stateMachine));
        }

        protected void HandleQuestlog()
        {
            stateMachine.SwitchState(new PlayerQuestlogState(stateMachine));
        }
    }

}
