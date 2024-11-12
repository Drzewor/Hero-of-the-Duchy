using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quests/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string questTitle;
        [SerializeField] public List<QuestStep> steps;
        [SerializeField] private Quest nextQuest;
        [SerializeField] public string id;
        public bool isFinished = false;

        public string GetTitle()
        {
            return questTitle;
        }

        public QuestStep GetQuestStep(int step)
        {
            return steps[step];
        }

        public Quest GetNextQuest()
        {
            if(nextQuest == null) return null;
            return nextQuest;
        }

        public void EndQuest()
        {
            isFinished = true;
        }
    }
}
