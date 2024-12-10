using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "QuestStepDialogue", menuName = "RPG/Quests/QuestStepDialogue", order = 1)]
    public class QuestStepDialogue : QuestStep
    {
        [SerializeField] private List<string> dialogueTriggerName;
        public override void TryToAdvance(object argument)
        {
            if(!(argument is string)) return;
            
            foreach(string questID in dialogueTriggerName)
            {
                if((string)argument == questID)
                {
                    stepProgress++;
                    if(stepProgress >= progressToFinish)
                    {
                        FinishQuestStep();
                    }
                }
            }
        }
    }
}
