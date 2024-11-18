using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerCraftingState : PlayerBaseState
{        
    private readonly int CraftingAnimationtHash = Animator.StringToHash("Crafting");
    private const float CrossFadeDuration = 0.5f;
    public PlayerCraftingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        EnterUIMode();
        stateMachine.InventoryManager.CraftingWindow.gameObject.SetActive(true);
        stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
        stateMachine.Animator.CrossFadeInFixedTime(CraftingAnimationtHash,CrossFadeDuration);

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
