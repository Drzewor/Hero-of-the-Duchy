using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using UnityEngine;

namespace RPG.StateMachine.Player
{
    public class PlayerDeadState : PlayerBaseState
    {
        private readonly int DeathAnimationHash = Animator.StringToHash("Death");
        private const float CrossFadeDuration = 0.1f;
        private const float TimeToLoadSave = 2f;
        private float timer = 0;
        private bool isLoading = false;
        public PlayerDeadState(PlayerStateMachine stateMachine) : base(stateMachine){}

        public override void Enter()
        {
            stateMachine.Animator.CrossFadeInFixedTime(DeathAnimationHash, CrossFadeDuration);
            stateMachine.WeaponLogic.gameObject.GetComponent<Collider>().enabled = false;
            stateMachine.WeaponLogic.ClearColidersList();
            stateMachine.WeaponLogic.SwitchTrails(false);
        }

        public override void Tick(float deltaTime)
        {
            timer += deltaTime;

            if(timer >= TimeToLoadSave && !isLoading)
            {
                isLoading = true;
                GameObject.FindObjectOfType<SavingWraper>().Load();
            }
        }

        public override void Exit()
        {
            
        }


    }

}
