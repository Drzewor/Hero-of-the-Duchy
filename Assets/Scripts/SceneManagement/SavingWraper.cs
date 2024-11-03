using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class SavingWraper : MonoBehaviour
    {
        [SerializeField] private float fadeIntime = 1.5f;
        private InputReader inputReader = null;
        private const string defaultSaveFile = "save";

        IEnumerator Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            inputReader = FindObjectOfType<InputReader>();
            inputReader.LoadEvent += Load;
            inputReader.SaveEvent += Save;
            inputReader.DeleteEvent += Delete;
            yield return fader.FadeIn(fadeIntime);
            
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
            ReSubscribeEvents();
        }
        
        public void Save()
        {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        private void Delete()
        {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        private void OnDestroy() 
        {
            inputReader.LoadEvent -= Load;
            inputReader.SaveEvent -= Save;
            inputReader.DeleteEvent -= Delete;
        }

        private void ReSubscribeEvents()
        {
            inputReader = FindObjectOfType<InputReader>();
            inputReader.LoadEvent -= Load;
            inputReader.SaveEvent -= Save;
            inputReader.DeleteEvent -= Delete;
            inputReader.LoadEvent += Load;
            inputReader.SaveEvent += Save;
            inputReader.DeleteEvent += Delete;
        }

    }
}
