using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerCraftingState : PlayerBaseState
    {        
        private readonly int CraftingAnimationtHash = Animator.StringToHash("Crafting");
        private const float CrossFadeDuration = 0.5f;
        private Transform snapingPosition;
        public PlayerCraftingState(PlayerStateMachine stateMachine, Transform snapingPosition) : base(stateMachine)
        {
            this.snapingPosition = snapingPosition;
        }

        public override void Enter()
        {
            EnterUIMode();
            stateMachine.InventoryManager.CraftingWindow.gameObject.SetActive(true);
            stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
            stateMachine.Animator.CrossFadeInFixedTime(CraftingAnimationtHash,CrossFadeDuration);

            stateMachine.transform.position = snapingPosition.position;
            stateMachine.transform.rotation = snapingPosition.rotation;

            stateMachine.InputReader.InteractEvent += OnExit;
            stateMachine.InputReader.PressESCEvent += OnExit;

            
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            ExitUIMode();
            stateMachine.InventoryManager.inventory.gameObject.SetActive(false);
            stateMachine.InventoryManager.CraftingWindow.gameObject.SetActive(false);
            stateMachine.InputReader.InteractEvent -= OnExit;
            stateMachine.InputReader.PressESCEvent -= OnExit;
        }

        private void OnExit()
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }
}

