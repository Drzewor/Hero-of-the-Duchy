using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerQuestlogState : PlayerBaseState
{
    public PlayerQuestlogState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        ShowMouseCursor();
        stateMachine.InventoryManager.QuestWindow.SetActive(true);

        stateMachine.InputReader.PressJEvent += OnExit;
        stateMachine.InputReader.PressESCEvent += OnExit;
    }

    public override void Tick(float deltaTime){}

    public override void Exit()
    {
        HideMouseCursor();
        stateMachine.InventoryManager.QuestWindow.SetActive(false);

        stateMachine.InputReader.PressJEvent -= OnExit;
        stateMachine.InputReader.PressESCEvent -= OnExit;
    }

    private void OnExit()
    {
        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

}
