using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using TMPro;
using UnityEngine;

namespace RPG.CharacterStats
{
    public class CharacterExperience : MonoBehaviour, ISaveable
    {
        private int level = 1;
        private int exp = 0;
        private int lerningPoints = 0;
        [SerializeField] private List<int> expToNextLevel;
        [SerializeField] private TMP_Text levelValueText;
        [SerializeField] private TMP_Text expValueText;
        [SerializeField] private TMP_Text LerningPointsValueText;

        private void OnEnable() 
        {
            UpdateDisplayLevel();
            UpdateDisplayExp();
            UpdateDisplayLernngPoints();
        }

        private void UpdateDisplayLevel()
        {
            levelValueText.text = level.ToString();
        }

        private void UpdateDisplayExp()
        {
            expValueText.text = $"{exp}/{expToNextLevel[level-1]}";
        }

        private void UpdateDisplayLernngPoints()
        {
            LerningPointsValueText.text = lerningPoints.ToString();
        }

        private void AddLerningPoints(int amount)
        {
            lerningPoints += amount;
            UpdateDisplayLernngPoints();
        }

        public bool SpendLerningPoints(int amount)
        {
            if(lerningPoints - amount < 0) return false;

            lerningPoints -= amount;
            UpdateDisplayLernngPoints();
            return true;
        }

        public void AddExp(int amount)
        {
            exp += amount;
            if(exp >= expToNextLevel[level-1] && level <= expToNextLevel.Count)
            {
                level++;
                AddLerningPoints(5);
                UpdateDisplayLevel();
            }
            UpdateDisplayExp();
        }

        public bool HasLerningPoints()
        {
            if(lerningPoints > 0)
            { 
                return true;
            }
            else
            {
                return false;
            }
        }

        public object CaptureState()
        {
            List<int> saveData = new List<int>();
            saveData.Add(level);
            saveData.Add(exp);
            saveData.Add(lerningPoints);
            return saveData;
        }

        public void RestoreState(object state)
        {
            List<int> saveData = (List<int>)state;
            level = saveData[0];
            exp = saveData[1];
            lerningPoints = saveData[2];

            UpdateDisplayLevel();
            UpdateDisplayExp();
            UpdateDisplayLernngPoints();
        }
    }
}

