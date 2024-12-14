using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

public class PlayerDialogueState : PlayerBaseState
{
    private readonly int IdleAnimationHash = Animator.StringToHash("Idle");
    private const float CrossFadeDuration = 0.3f;
    public PlayerDialogueState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
        EnterUIMode();
        stateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
    }

    public override void Exit()
    {
        ExitUIMode();
    }
}
