using System.Collections;
using System.Collections.Generic;
using RPG.SceneManagement;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SaveLoadUI : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private GameObject buttonPrefab;

    private void OnEnable() 
    {
        SavingWraper savingWraper = FindObjectOfType<SavingWraper>();
        if(savingWraper == null) return;
        
        if(contentRoot.childCount != 0)
        {
            foreach(Transform child in contentRoot)
            {
                Destroy(child.gameObject);
            }
        }

        foreach(string save in savingWraper.ListSaves())
        {
            GameObject buttonInstance = Instantiate(buttonPrefab,contentRoot);
            TMP_Text textComp = buttonInstance.GetComponentInChildren<TMP_Text>();
            textComp.text = save;
            Button button = buttonInstance.GetComponentInChildren<Button>();
            button.onClick.AddListener(() =>
            {
                savingWraper.LoadGame(save);
            });
        }

    }
}
