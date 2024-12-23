using System;
using System.Collections;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus 
    {
        private Quest quest;
        public int stepInProgress;
        public int progressOfStep;
        public bool isFinished;

        public QuestStatus(Quest quest, int stepInProgress = 0, int progressOfStep = 0, bool isQuestFinished = false)
        {
            this.quest = quest;
            this.isFinished = isQuestFinished;
            this.stepInProgress = stepInProgress;
            this.progressOfStep = progressOfStep;
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public QuestStep GetCurrentQuestStep()
        {
            return quest.GetQuestStep(stepInProgress);
        }

        public void TryToAdvance(object argument)
        {
            if(quest.GetQuestStep(stepInProgress).TryToAdvance(argument))
            {
                progressOfStep++;
            }
        }

        public bool TryMoveToNextQuestStep()
        {
            bool MovedToNextStep = false;
            if(quest.GetQuestStep(stepInProgress).progressToFinish <= progressOfStep)
            {
                stepInProgress++;
                progressOfStep = 0;
                MovedToNextStep = true;
            }
            if(stepInProgress >= quest.steps.Count)
            {
                isFinished = true;
                MovedToNextStep = false;
            }

            return MovedToNextStep;
        }

        public bool CheckPreCompletion()
        {
            if(quest.GetQuestStep(stepInProgress).CheckQuestStepPreCompletion() != null)
            {
                progressOfStep = (int)quest.GetQuestStep(stepInProgress).CheckQuestStepPreCompletion();
                return true;
            } 
            return false;
        }
    }
}