using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatIncreaseButton : MonoBehaviour
{
    [SerializeField] private CharacterExperience characterExperience;
    private Button button;
    private void OnEnable() 
    {
        button = GetComponent<Button>();

        if(characterExperience.HasLerningPoints())
        {
            button.image.enabled = true;
            button.enabled = true;
        }
        else
        {
            button.image.enabled = false;
            button.enabled = false;
        }
    }

    public void TryHideButton()
    {
        if(!characterExperience.HasLerningPoints())
        {
            button.image.enabled = false;
            button.enabled = false;
        }
    }

    
}
