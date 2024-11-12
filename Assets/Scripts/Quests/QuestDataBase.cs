using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using UnityEngine;

[CreateAssetMenu(fileName = "New QuestDataBase", menuName = "RPG/Quests/QuestDataBase", order = 2)]
public class QuestDataBase : ScriptableObject
{
    [SerializeField] public List<Quest> quests;
    
    public Quest GetQuestById(string id)
    {
        foreach(Quest quest in quests)
        {
            if(quest.id == id)
            {
                return quest;
            }
        }
        return null;
    }
    
}
