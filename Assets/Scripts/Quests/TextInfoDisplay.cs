using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace RPG.Quests
{
    public class TextInfoDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject textPrefab;
        [SerializeField] private float textLifeTime = 4;
        private List<GameObject> texts = new List<GameObject>();
        private float timer = Mathf.Infinity; 

        private void Update() 
        {
            if(texts.Count == 0) return;
            timer += Time.deltaTime;
            if(timer >= textLifeTime)
            {
                GameObject textToDestroy = texts[0];
                texts.RemoveAt(0);
                Destroy(textToDestroy);
                timer = 0;
            }
            
        }

        public void DisplayQuestAddInfo(string questTitle)
        {

            Display($"Quest \"{questTitle}\" added to QuestLog [<color=yellow>J</color>]");
            
        }

        public void DisplayQuestStepFinish(string questTitle)
        {
            Display($"You move to next step of quest \"{questTitle}\" [<color=yellow>J</color>]");
        }

        public void DisplayQuestFinish(string questTitle)
        {
            Display($"You finished \"{questTitle}\" quest");
        }

        private void Display(string info)
        {
            if(texts.Count == 0)
            {
                timer = 0;
            }
            GameObject newText = Instantiate(textPrefab,gameObject.transform);
            newText.GetComponent<TMP_Text>().text = info;
            texts.Add(newText);
        }
    }
}

