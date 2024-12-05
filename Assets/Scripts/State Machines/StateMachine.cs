using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine
{
    public abstract class StateMachine : MonoBehaviour
    {
        private State currentState;

        public void SwitchState(State newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState?.Enter();
        }
        
        public State GetCurrentState()
        {
            return currentState;
        }

        protected virtual void Update()
        {
            currentState?.Tick(Time.deltaTime);
        }

    }
}

