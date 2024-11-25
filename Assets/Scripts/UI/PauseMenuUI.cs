using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private void OnEnable() 
        {
            Time.timeScale = 0;
        }

        private void OnDisable() 
        {
            Time.timeScale = 1;
        }

        public void Resume()
        {
            FindObjectOfType<PlayerStateMachine>()?.SwitchPauseMenu();
        }

        public void Save()
        {
            FindObjectOfType<SavingWraper>().Save();
        }

        public void SaveAndQuit()
        {
            Save();
            FindObjectOfType<SavingWraper>().LoadMenu();
        }
    }
}