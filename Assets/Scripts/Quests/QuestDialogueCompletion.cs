using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestDialogueCompletion : MonoBehaviour
    {
        [SerializeField] string dialogueTriggerName;

        public void CompleteDialogueQuestStep()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>().TryToAdvanceQuests(dialogueTriggerName);
        }

    }

}
