using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatButtonsControler : MonoBehaviour
{
    [SerializeField] private List<Button> buttons;
    [SerializeField] private CharacterExperience characterExperience;

    private void OnEnable() 
    {
        if(characterExperience.HasLerningPoints())
        {
            foreach(Button button in buttons)
            {   
                button.image.enabled = true;
                button.enabled = true;
            }
        }
        else
        {
            foreach(Button button in buttons)
            {   
                button.image.enabled = false;
                button.enabled = false;
            }
        }
    }


    public void TryHideButton()
    {
        if(!characterExperience.HasLerningPoints())
        {
            foreach(Button button in buttons)
            {   
                button.image.enabled = false;
                button.enabled = false;
            }
        }
    }
}
