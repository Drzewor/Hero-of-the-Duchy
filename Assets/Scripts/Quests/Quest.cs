using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quests/Quest", order = 0)]
    public class Quest : ScriptableObject
    {
        [SerializeField] private string questTitle;
        [SerializeField] private Quest nextQuest;
        [SerializeField] public List<QuestStep> steps;
        [SerializeField] public int expReward;
        [SerializeField] public List<Item> itemReward;
        [SerializeField] public string id;

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
    }
}
