using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

namespace RPG.UI
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TMP_Text title;
        [SerializeField] TMP_Text progress;
        public void Setup(Quest quest)
        {
            title.text = quest.GetTitle();
            progress.text = $"0/{quest.GetObjectiveCount()}";
        }
    }
}
