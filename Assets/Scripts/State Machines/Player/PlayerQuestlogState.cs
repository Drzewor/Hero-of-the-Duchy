using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerQuestlogState : PlayerBaseState
    {
        private readonly int IdleAnimationHash = Animator.StringToHash("Idle");
        private const float CrossFadeDuration = 0.3f;
        public PlayerQuestlogState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            EnterUIMode();
            stateMachine.InventoryManager.QuestWindow.SetActive(true);
            stateMachine.Animator.CrossFadeInFixedTime(IdleAnimationHash,CrossFadeDuration);

            stateMachine.InputReader.PressJEvent += OnExit;
            stateMachine.InputReader.PressESCEvent += OnExit;
        }

        public override void Tick(float deltaTime){}

        public override void Exit()
        {
            ExitUIMode();
            stateMachine.InventoryManager.QuestWindow.SetActive(false);

            stateMachine.InputReader.PressJEvent -= OnExit;
            stateMachine.InputReader.PressESCEvent -= OnExit;
        }

        private void OnExit()
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

    }
}

