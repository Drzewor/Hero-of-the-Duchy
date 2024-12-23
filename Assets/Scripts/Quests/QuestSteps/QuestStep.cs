using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public abstract class QuestStep : ScriptableObject
    {
        [SerializeField, TextArea(10,20)] private string stepDescription;
        protected int stepProgress = 0;
        [SerializeField] public int progressToFinish = 1;
        [SerializeField] private string stepID = "ID is not always required";

        public string GetDescription()
        {
            return stepDescription;
        }

        public string GetStepId()
        {
            return stepID;
        }

        public virtual int? CheckQuestStepPreCompletion()
        {
            return null;
        }

        public abstract bool TryToAdvance(object argument);
    }
}