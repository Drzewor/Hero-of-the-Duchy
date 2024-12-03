using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestDialogueCompletion : MonoBehaviour
    {
        [SerializeField] List<string> dialogueTriggerName;

        public void CompleteDialogueQuestStep(int index)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>().TryToAdvanceQuests(dialogueTriggerName[index]);
        }

    }

}
