using System.Collections;
using System.Collections.Generic;
using RPG.CharacterStats;
using UnityEngine;

namespace RPG.Inventories
{
    public class StatPanel : MonoBehaviour
    {
        [SerializeField] StatDisplay[] statDisplays;
        [SerializeField] string[] statNames;

        private CharacterStat[] stats;

        private void OnValidate() 
        {
            statDisplays = GetComponentsInChildren<StatDisplay>();
            UpdateStatNames();
        }

        public void SetStats(params CharacterStat[] charStats)
        {
            stats = charStats;

            if(stats.Length > statDisplays.Length)
            {
                Debug.LogError("Not enough Stat Displays");
                return;
            }

            for (int i = 0; i < statDisplays.Length; i++)
            {
                statDisplays[i].gameObject.SetActive(i < stats.Length);

                if(i < statDisplays.Length)
                {
                    statDisplays[i].Stat = stats[i];
                }

            }
        }

        public void UpdateStatValues()
        {
            for (int i = 0; i < stats.Length; i++)
            {
                statDisplays[i].UpdateStatValue();
            }
        }

        public void UpdateStatNames()
        {
            for (int i = 0; i < statNames.Length; i++)
            {
                statDisplays[i].Name = statNames[i];
            }
        }
    }
}

