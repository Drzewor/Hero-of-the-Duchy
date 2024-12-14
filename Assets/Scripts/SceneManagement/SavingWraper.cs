using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class SavingWraper : MonoBehaviour
    {
        [SerializeField] private float fadeIntime = 1.5f;
        private const int FirstLevelBuildIndex = 1;
        private const int MenuBuildIndex = 0;
        private const int EndSceneBuildIndex = 2;
        private const string currentSaveKey = "currentSaveName";
        
        public void ContinueGame()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return;
            if(!GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave())) return;
            StartCoroutine(LoadLastScene());
        }

        public void NewGame(string saveFile)
        {
            if(String.IsNullOrEmpty(saveFile)) return;
            SetCurrentSave(saveFile);
            StartCoroutine(LoadChosenScene(FirstLevelBuildIndex));
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadChosenScene(MenuBuildIndex));
        }

        public void LoadEndScene()
        {
            StartCoroutine(LoadChosenScene(EndSceneBuildIndex));
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeIntime);
            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeIntime);
        }

        private IEnumerator LoadChosenScene(int index)
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeIntime);
            yield return SceneManager.LoadSceneAsync(index);
            yield return fader.FadeIn(fadeIntime);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }
        
        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }
    }
}
