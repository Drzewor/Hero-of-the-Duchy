using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "QuestStepKill", menuName = "RPG/Quests/QuestStepKill", order = 0)]
    public class QuestStepKill : QuestStep
    {
        [SerializeField] private List<string> targetsNames;

        public override void TryToAdvance(object argument)
        {
            if(!(argument is Health)) return;
            
            foreach(string target in targetsNames)
            {
                if(((Health)argument).gameObject.name != target) continue;

                stepProgress++;
                
                if(stepProgress >= progressToFinish)
                {
                    FinishQuestStep();
                }
            }

        }
    }
}
