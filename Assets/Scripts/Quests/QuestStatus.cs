using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus 
    {
        private Quest quest;
        public int stepInProgress;
        public int progressOfStep;

        public QuestStatus(Quest quest, int stepInProgress = 0, int progressOfStep = 0, bool isQuestFinished = false)
        {
            this.quest = quest;
            this.quest.isFinished = isQuestFinished;
            this.stepInProgress = stepInProgress;
            this.progressOfStep = progressOfStep;
            for(int i = 0; i < quest.steps.Count; i++)
            {
                if(i < stepInProgress)
                {
                    quest.steps[i].isFinished = true;
                    continue;
                }
                else if (i == stepInProgress)
                {
                    quest.steps[i].isFinished = false;
                    quest.steps[i].SetStepProgress(this.progressOfStep);
                    continue;
                }
                else if(i > stepInProgress)
                {
                    quest.steps[i].SetStepProgress(0);
                    quest.steps[i].isFinished = false;
                }
                
            }
        }

        public Quest GetQuest()
        {
            return quest;
        }

        public QuestStep GetCurrentQuestStep()
        {
            return quest.GetQuestStep(stepInProgress);
        }

        public bool TryMoveToNextQuestStep()
        {
            progressOfStep = quest.GetQuestStep(stepInProgress).GetStepProgress();
            bool MovedToNextStep = false;
            if(quest.GetQuestStep(stepInProgress).isFinished)
            {
                stepInProgress += 1;
                progressOfStep = 0;
                MovedToNextStep = true;
            }
            if(stepInProgress >= quest.steps.Count)
            {
                quest.EndQuest();
                MovedToNextStep = false;
            }

            return MovedToNextStep;
        }
    }
}