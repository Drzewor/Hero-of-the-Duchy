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
        stateMachine.Character.inventory.gameObject.SetActive(true);
        stateMachine.Character.EquipmentStats.gameObject.SetActive(true);

        stateMachine.InputReader.PressIEvent += OnExit;
        stateMachine.InputReader.PressESCEvent += OnExit;
    }

    public override void Tick(float deltaTime){}

    public override void Exit()
    {
        HideMouseCursor();
        stateMachine.Character.inventory.gameObject.SetActive(false);
        stateMachine.Character.EquipmentStats.gameObject.SetActive(false);

        stateMachine.InputReader.PressIEvent -= OnExit;
        stateMachine.InputReader.PressESCEvent -= OnExit;
    }
    
    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }
}
