using System.Collections;
using System.Collections.Generic;
using RPG.Combat;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    [SerializeField] TMP_Text healthDisplay;
    [SerializeField] Health health;
    [SerializeField] Character character;

    private void Start() 
    {
        character.OnMaxHealthUpdate += OnEnable;
    }

    private void OnEnable() 
    {
        healthDisplay.text = $"{health.GeHealthPoints()}/{character.MaxHealth.Value}";
    }
}
