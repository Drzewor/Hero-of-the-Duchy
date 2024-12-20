using System;
using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine;
using RPG.StateMachine.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RPG.StateMachine.Player
{
    public class PlayerPauseState : PlayerBaseState
    {
        private Type previousState;
        public PlayerPauseState(PlayerStateMachine stateMachine, Type previousState) : base(stateMachine)
        {
            this.previousState = previousState;
        }

        public override void Enter()
        {
            stateMachine.InputReader.PressESCEvent += HandleExit;
        }

        public override void Tick(float deltaTime){}

        public override void Exit()
        {

        }

        
        private void HandleExit()
        {
            // if(previousState == typeof(PlayerFreeLookState))
            // {
            //     stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            // }
            // if(previousState == typeof(PlayerTargetingState))
            // {
            //     stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            // }
            // if(previousState == typeof(PlayerSprintingState))
            // {
            //     stateMachine.SwitchState(new PlayerSprintingState(stateMachine));
            // }
            // if(previousState == typeof(PlayerJumpingState))
            // {
            //     stateMachine.SwitchState(new PlayerJumpingState(stateMachine));
            // }
        }

    }
}

