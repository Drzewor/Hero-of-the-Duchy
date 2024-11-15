using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{
    [Serializable]
    public class QuestSaveData
    { 
        public string questID;
        public int stepInProgress;
        public int progressOfStep;
        public bool isFinished;

        public QuestSaveData(string questID, int stepInProgress, int progressOfStep, bool isFinished)
        {
            this.questID = questID;
            this.stepInProgress = stepInProgress;
            this.progressOfStep = progressOfStep;
            this.isFinished = isFinished;
        }
    }

    [Serializable]
    public class QuestListSaveData
    {
        public List<QuestSaveData> questSaveDatas;

        public QuestListSaveData(List<QuestSaveData> questSaveDatas)
        {
            this.questSaveDatas = questSaveDatas;
        }
    }
}
