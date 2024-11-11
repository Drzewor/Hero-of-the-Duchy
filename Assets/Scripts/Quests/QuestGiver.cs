using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] Quest[] quest;

        public void GiveQuest(int index)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestManager>().AddQuest(quest[index]);
        }

    }

}