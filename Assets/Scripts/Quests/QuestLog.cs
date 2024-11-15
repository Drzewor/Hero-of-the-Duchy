using System.Collections;
using System.Collections.Generic;
using RPG.Quests;
using TMPro;
using UnityEngine;

public class QuestLog : MonoBehaviour
{
    [SerializeField] private RectTransform questList;
    [SerializeField] private TMP_Text questTittle;
    [SerializeField] private TMP_Text log;
    [SerializeField] private GameObject questButtonPrefab;
    [SerializeField] private QuestManager questManager;
    private void OnEnable() {
        if(questList.childCount > 0)
        {
            foreach(Transform transform in questList.transform)
            {
                Destroy(transform.gameObject);
            }
        }
        
        if(questManager.GetQuests().Count == 0) return;

        GameObject questButton;

        foreach(QuestStatus questStatus in questManager.GetQuests())
        {
            questButton = Instantiate(questButtonPrefab,questList);
            questButton.GetComponent<QuestLogButton>().SetButton(questStatus, questTittle, log);
            if(questStatus.GetQuest().isFinished)
            {
                questButton.GetComponentInChildren<TMP_Text>().color = Color.green;
            }
        }
    }
}