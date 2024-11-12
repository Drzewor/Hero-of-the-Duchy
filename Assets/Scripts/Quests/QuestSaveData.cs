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

        public QuestSaveData(string questID, int stepInProgress, int progressOfStep)
        {
            this.questID = questID;
            this.stepInProgress = stepInProgress;
            this.progressOfStep = progressOfStep;
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
