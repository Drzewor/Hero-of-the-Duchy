using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestManager : MonoBehaviour, IPredicateEvaluator, ISaveable
    {
        private List<QuestStatus> quests = new List<QuestStatus>();
        [SerializeField] private QuestDataBase questDataBase;

        public List<QuestStatus> GetQuests()
        {
            return quests;
        }

        public void AddQuest(Quest quest)
        {
            quests.Add(new QuestStatus(quest));
        }

        private void Clear()
        {
            quests.Clear();
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

        public object CaptureState()
        {
            List<QuestSaveData> questSaveDatas = new List<QuestSaveData>();

            if(quests.Count == 0) return null;

            foreach(QuestStatus status in quests)
            {
                Debug.Log(status.progressOfStep);
                QuestSaveData questSave = new QuestSaveData(status.GetQuest().id, status.stepInProgress, status.progressOfStep);
                questSaveDatas.Add(questSave);
            }

            QuestListSaveData saveData = new QuestListSaveData(questSaveDatas);

            return saveData;
        }

        public void RestoreState(object state)
        {
            QuestListSaveData saveData = (QuestListSaveData)state;

            Clear();

            if(saveData == null) return;

            foreach(QuestSaveData savedQuest in saveData.questSaveDatas)
            {
                Debug.Log($"step{savedQuest.stepInProgress} progress{savedQuest.progressOfStep}");
                QuestStatus questStatus = new QuestStatus
                (
                    questDataBase.GetQuestById(savedQuest.questID), 
                    savedQuest.stepInProgress, 
                    savedQuest.progressOfStep
                );
                quests.Add(questStatus);
            }
        }
    }
}