using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerDialogueState : PlayerBaseState
{
    public PlayerDialogueState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        EnterUIMode();
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        ExitUIMode();
    }
}
