using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestManager : MonoBehaviour, IPredicateEvaluator
    {
        private List<QuestStatus> quests = new List<QuestStatus>();

        public List<QuestStatus> GetQuests()
        {
            return quests;
        }

        public void AddQuest(Quest quest)
        {
            quests.Add(new QuestStatus(quest));
        }

        public void TryToAdvanceQuests(object argument)
        {
            if(quests.Count == 0) return;
            foreach(QuestStatus questStatus in quests)
            {
                if(questStatus.GetQuest().isFinished) return;
                questStatus.GetCurrentQuestStep().TryToAdvance(argument);
                questStatus.TryMoveToNextQuestStep();
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            switch (predicate)
            {
                case "HasQuest":
                    foreach(QuestStatus questStatus in quests)
                    {
                        if(questStatus.GetQuest().name == parameters[0])
                        {
                            return true;
                        }
                    }
                    return false;
                case "CompletedQuest":
                    foreach(QuestStatus questStatus in quests)
                    {
                        if(questStatus.GetQuest().name == parameters[0])
                        {
                            return questStatus.GetQuest().isFinished;
                        }
                    }
                    return false;
                default:
                    return null;
            }

            
        }
    }
}