using System;
using System.Collections;
using System.Collections.Generic;
using RPG.CharacterStats;
using RPG.Core;
using RPG.Inventories;
using RPG.Saving;
using TMPro;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestManager : MonoBehaviour, IPredicateEvaluator, ISaveable
    {
        private List<QuestStatus> quests = new List<QuestStatus>();
        [SerializeField] private QuestDataBase questDataBase;
        [SerializeField] private TextInfoDisplay questInfoDisplay;
        [SerializeField] private QuestEventActivator eventActivator;

        public List<QuestStatus> GetQuests()
        {
            return quests;
        }

        public void AddQuest(Quest quest)
        {
            quests.Add(new QuestStatus(quest));
            
            eventActivator.CheckForTrigger(quest.id, QuestEventActivator.TriggerType.QuestStart);

            questInfoDisplay.DisplayQuestAddInfo(quest.GetTitle());

            if(quests[quests.Count-1].CheckPreCompletion())
            {
                TryToAdvanceQuests(null);
            }
        }

        private void Clear()
        {
            quests.Clear();
        }

        public void TryToAdvanceQuests(object argument)
        {
            if(quests.Count == 0) return;

            for(int i = 0; i < quests.Count; i++)
            {
                if(quests[i].isFinished) continue;

                quests[i].TryToAdvance(argument);
                
                if(quests[i].TryMoveToNextQuestStep())
                {
                    eventActivator.CheckForTrigger(quests[i].GetQuest().GetQuestStep(quests[i].stepInProgress-1).GetStepId(), 
                    QuestEventActivator.TriggerType.StepComplete);
                    
                    questInfoDisplay.DisplayQuestStepFinish(quests[i].GetQuest().GetTitle());

                    eventActivator.CheckForTrigger(quests[i].GetQuest().GetQuestStep(quests[i].stepInProgress).GetStepId(), 
                    QuestEventActivator.TriggerType.StepStart);

                    if(quests[i].CheckPreCompletion())
                    {
                        TryToAdvanceQuests(null);
                    }
                }

                if(quests[i].isFinished)
                {
                    GiveRewards(quests[i].GetQuest());

                    eventActivator.CheckForTrigger(quests[i].GetQuest().GetQuestStep(quests[i].stepInProgress-1).GetStepId(), 
                    QuestEventActivator.TriggerType.StepComplete);

                    eventActivator.CheckForTrigger(quests[i].GetQuest().id, 
                    QuestEventActivator.TriggerType.QuestComplete);

                    questInfoDisplay.DisplayQuestFinish(quests[i].GetQuest().GetTitle());

                    Quest nextQuest = quests[i].GetQuest().GetNextQuest();
                    if(nextQuest == null) continue;
                    AddQuest(nextQuest);
                }
            }
        }

        private void GiveRewards(Quest quest)
        {
            GetComponent<CharacterExperience>().AddExp(quest.expReward);

            if(quest.itemReward.Count == 0) return;
            Inventory inventory = GetComponent<InventoryManager>().inventory;
            ItemDropper itemDropper = GetComponent<ItemDropper>();
            foreach(Item item in quest.itemReward)
            {
                if(inventory.CanAddItem(item))
                {
                    inventory.AddItem(item);
                }
                else
                {
                    itemDropper.DropItem(item);
                }
            }
        }

        public bool? Evaluate(predicateName predicate, string[] parameters)
        {
            switch (predicate)
            {
                case predicateName.HasQuest:
                    foreach(QuestStatus questStatus in quests)
                    {
                        if(questStatus.GetQuest().id == parameters[0])
                        {
                            return true;
                        }
                    }
                    return false;
                case predicateName.CompletedQuest:
                    foreach(QuestStatus questStatus in quests)
                    {
                        if(questStatus.GetQuest().id == parameters[0])
                        {
                            return questStatus.isFinished;
                        }
                    }
                    return false;
                case predicateName.IsQuestStepActive:
                    foreach(QuestStatus questStatus in quests)
                    {
                        if(questStatus.isFinished) continue;
                        if(questStatus.GetCurrentQuestStep().GetStepId() == parameters[0])
                        {
                            return true;
                        }
                    }
                    return false;
                case predicateName.CompletedQuestStep:
                    foreach(QuestStatus questStatus in quests)
                    {
                        for(int i = 0; i < questStatus.GetQuest().steps.Count; i++)
                        {
                            if(questStatus.GetQuest().steps[i].GetStepId() == parameters[0])
                            {
                                return questStatus.stepInProgress > i; 
                            }
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
                QuestSaveData questSave = new QuestSaveData(status.GetQuest().id, status.stepInProgress, status.progressOfStep, status.isFinished);
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
                QuestStatus questStatus = new QuestStatus
                (
                    questDataBase.GetQuestById(savedQuest.questID), 
                    savedQuest.stepInProgress, 
                    savedQuest.progressOfStep,
                    savedQuest.isFinished 
                    
                );
                quests.Add(questStatus);
            }
        }
    }
}