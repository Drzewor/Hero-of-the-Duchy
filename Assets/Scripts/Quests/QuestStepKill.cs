using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.iOS;

namespace RPG.Quests
{
    [CreateAssetMenu(fileName = "QuestStepKill", menuName = "RPG/Quests/QuestStepKill", order = 0)]
    public class QuestStepKill : QuestStep
    {
        [SerializeField] private string targetName;

        public override void TryToAdvance(object argument)
        {
            if(!(argument is string)) return;

            if((string)argument != targetName) return;
            
            stepProgress++;

            if(stepProgress >= progressToFinish)
            {
                FinishQuestStep();
            }
            
        }
    }
}
