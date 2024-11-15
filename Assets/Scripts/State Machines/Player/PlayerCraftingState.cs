using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerCraftingState : PlayerBaseState
{
    public PlayerCraftingState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        ShowMouseCursor();
        stateMachine.InventoryManager.CraftingWindow.gameObject.SetActive(true);
        stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
        stateMachine.InputReader.PressESCEvent += OnExit;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        HideMouseCursor();
        stateMachine.InventoryManager.inventory.gameObject.SetActive(false);
        stateMachine.InventoryManager.CraftingWindow.gameObject.SetActive(false);
        stateMachine.InputReader.PressESCEvent -= OnExit;
    }

    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
