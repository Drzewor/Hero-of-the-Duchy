using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestStatus 
    {
        private Quest quest;
        private int stepInProgress;
        private int progressOfStep;

        public QuestStatus(Quest quest, int stepInProgress = 0, int progressOfStep = 0)
        {
            this.quest = quest;
            this.quest.isFinished = false;
            this.stepInProgress = stepInProgress;
            this.progressOfStep = progressOfStep;
            foreach(QuestStep questStep in quest.steps)
            {
                questStep.isFinished = false;
                questStep.SetStepProgress(this.progressOfStep);
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

        public void TryMoveToNextQuestStep()
        {
            if(quest.GetQuestStep(stepInProgress).isFinished)
            {
                stepInProgress += 1;
            }
            if(stepInProgress >= quest.steps.Count)
            {
                quest.EndQuest();
            }
        }
    }
}