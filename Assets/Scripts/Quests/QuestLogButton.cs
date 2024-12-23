using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RPG.Quests;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace RPG.Quests
{
    public class QuestLogButton : MonoBehaviour
    {
        private Button button;
        public QuestStatus questStatus;
        [SerializeField] private TMP_Text buttonText;
        private TMP_Text tittle;
        private TMP_Text log;
        private static readonly StringBuilder sb = new StringBuilder();

        public void SetButton(QuestStatus questStatus, TMP_Text tittle, TMP_Text log)
        {
            this.questStatus = questStatus;
            buttonText.text = questStatus.GetQuest().GetTitle();
            this.tittle = tittle;
            this.log = log;
        }

        public void FillQuestLog()
        {
            tittle.text = questStatus.GetQuest().GetTitle();

            sb.Length = 0;
            for(int i = 0; i < questStatus.GetQuest().steps.Count; i++)
            {
                if(sb.Length > 0) sb.AppendLine();
                sb.Append(questStatus.GetQuest().steps[i].GetDescription());
                sb.AppendLine();
                if(i >= questStatus.stepInProgress) break;
            }
            log.text = sb.ToString();
        }
    }

}
