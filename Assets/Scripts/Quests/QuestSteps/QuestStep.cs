using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public abstract class QuestStep : ScriptableObject
    {
        public bool isFinished = false;
        [SerializeField, TextArea(10,20)] private string stepDescription;
        protected int stepProgress = 0;
        [SerializeField] protected int progressToFinish = 1;
        [SerializeField] private string stepID = "ID is not always required";

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

        public string GetStepId()
        {
            return stepID;
        }

        public abstract void TryToAdvance(object argument);
    }
}