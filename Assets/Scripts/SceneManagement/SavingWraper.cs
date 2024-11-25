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
        // private InputReader inputReader = null;
        private const int FirstLevelBuildIndex = 1;
        private const int MenuBuildIndex = 0;
        private const string currentSaveKey = "currentSaveName";

        // private void Start()
        // {
        //     SubscribeEvents();
        // }
        
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

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        // private void SubscribeEvents()
        // {
        //     inputReader = FindObjectOfType<InputReader>();

        //     inputReader.LoadEvent += Load;
        //     inputReader.SaveEvent += Save;
        //     inputReader.DeleteEvent += Delete;    
        // }

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
            // ReSubscribeEvents();
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

        // private void OnDestroy() 
        // {
        //     if(inputReader == null) return;

        //     inputReader.LoadEvent -= Load;
        //     inputReader.SaveEvent -= Save;
        //     inputReader.DeleteEvent -= Delete;
        // }

        // private void ReSubscribeEvents()
        // {
        //     inputReader = FindObjectOfType<InputReader>();
        //     if(inputReader == null) return;

        //     inputReader.LoadEvent -= Load;
        //     inputReader.SaveEvent -= Save;
        //     inputReader.DeleteEvent -= Delete;
        //     inputReader.LoadEvent += Load;
        //     inputReader.SaveEvent += Save;
        //     inputReader.DeleteEvent += Delete;
        // }

    }
}
