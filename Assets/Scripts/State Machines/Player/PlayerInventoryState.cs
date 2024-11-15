using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerInventoryState : PlayerBaseState
{
    public PlayerInventoryState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        ShowMouseCursor();
        stateMachine.InventoryManager.inventory.gameObject.SetActive(true);
        stateMachine.InventoryManager.EquipmentStats.gameObject.SetActive(true);

        stateMachine.InputReader.PressIEvent += OnExit;
        stateMachine.InputReader.PressESCEvent += OnExit;
    }

    public override void Tick(float deltaTime){}

    public override void Exit()
    {
        HideMouseCursor();
        stateMachine.InventoryManager.inventory.gameObject.SetActive(false);
        stateMachine.InventoryManager.EquipmentStats.gameObject.SetActive(false);

        stateMachine.InputReader.PressIEvent -= OnExit;
        stateMachine.InputReader.PressESCEvent -= OnExit;
    }
    
    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
