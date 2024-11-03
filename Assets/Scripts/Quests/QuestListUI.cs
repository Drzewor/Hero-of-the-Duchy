using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Quests;

namespace RPG.UI
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] Quest[] tempQuests;
        [SerializeField] QuestItemUI questPrefab;

        private void Start() 
        {
               foreach(Quest quest in tempQuests)
               {
                    QuestItemUI uiInstatnce = Instantiate<QuestItemUI>(questPrefab, gameObject.transform);
                    uiInstatnce.Setup(quest);
               }
        }
    }
}
