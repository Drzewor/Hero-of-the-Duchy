using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using UnityEngine;

namespace RPG.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] Health health;
        [SerializeField] GameObject bar;
        [SerializeField] RectTransform foreground;
        [SerializeField] bool isPlayer = false;

        void Update()
        {
            if(!isPlayer)
            {
                if(health.GetPercentageOfHealth() <= 0f || health.GetPercentageOfHealth() >= 1f)
                {
                    bar.SetActive(false);
                    return;
                }
                else if(!bar.activeInHierarchy)
                {
                    bar.SetActive(true);
                }
            }
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            foreground.localScale = new Vector3(health.GetPercentageOfHealth(), 1, 1);
        }
    }
}

