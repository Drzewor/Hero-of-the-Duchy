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
        stateMachine.Character.CraftingWindow.gameObject.SetActive(true);
        stateMachine.Character.inventory.gameObject.SetActive(true);
        stateMachine.InputReader.PressESCEvent += OnExit;
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        HideMouseCursor();
        stateMachine.Character.inventory.gameObject.SetActive(false);
        stateMachine.Character.CraftingWindow.gameObject.SetActive(false);
        stateMachine.InputReader.PressESCEvent -= OnExit;
    }

    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
