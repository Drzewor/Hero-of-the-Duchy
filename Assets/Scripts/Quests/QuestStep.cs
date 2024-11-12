using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public abstract class QuestStep : ScriptableObject
    {
        public bool isFinished = false;
        [SerializeField] private string stepDescription;
        protected int stepProgress = 0;
        [SerializeField] protected int progressToFinish;

        protected void FinishQuestStep()
        {
            isFinished = true;
        }

        public string GetDescription()
        {
            return stepDescription;
        }

        public void SetStepProgress(int progress)
        {
            stepProgress = progress;
            if(stepProgress >= progressToFinish)
            {
                FinishQuestStep();
            }
        }

        public int GetStepProgress()
        {
            return stepProgress;
        }

        public abstract void TryToAdvance(object argument);
    }
}