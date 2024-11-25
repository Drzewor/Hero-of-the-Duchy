using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField newGameNameField;
        private SavingWraper savingWraper = null;
        public void ContinueGame()
        {
            if(savingWraper == null)
            {
                savingWraper = FindObjectOfType<SavingWraper>();
            }
            savingWraper.ContinueGame();
        }

        public void NewGame()
        {
            if(savingWraper == null)
            {
                savingWraper = FindObjectOfType<SavingWraper>();
            }
            savingWraper.NewGame(newGameNameField.text);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}

