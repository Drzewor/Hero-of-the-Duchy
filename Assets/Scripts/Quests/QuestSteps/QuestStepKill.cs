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

        public override bool TryToAdvance(object argument)
        {
            if(!(argument is Health)) return false;
            
            foreach(string target in targetsNames)
            {
                if(((Health)argument).gameObject.name != target) continue;

                return true;
            }

            return false;
        }
    }
}
