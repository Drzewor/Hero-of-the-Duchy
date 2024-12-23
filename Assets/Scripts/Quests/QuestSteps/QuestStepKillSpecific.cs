using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using RPG.Quests;
using RPG.Saving;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "QuestStepKillSpecific", menuName = "RPG/Quests/QuestStepKillSpecific")]
public class QuestStepKillSpecific : QuestStep 
{
    [SerializeField] private List<string> targetsID;
    [SerializeField] private int QuestSceneIndex;

    public override int? CheckQuestStepPreCompletion()
    {
        if(QuestSceneIndex != SceneManager.GetActiveScene().buildIndex) return null;

        int progress = 0;
        Health[] healths = FindObjectsOfType<Health>();
        if(healths.Length == 0) return progress;

        foreach(Health health in healths)
        {
            if(!health.isDead) continue;

            if(TryToAdvance(health))
            {
                progress++;
            }
        }
        return progress;
    }

    public override bool TryToAdvance(object argument)
    {
        if(!(argument is Health)) return false;
        if(!((Health)argument).TryGetComponent<SaveableEntity>(out SaveableEntity entity)) return false;

        foreach(string target in targetsID)
        {
            if(entity.GetUniqueIdentifier() != target) continue;

            return true;
        }
        return false;
    }
}
