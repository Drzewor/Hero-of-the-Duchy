using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.NPC;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IInteractable
    {
        [SerializeField] private string conversantName = "NPC Name";
        [SerializeField] private List<Dialogue> dialogues;

        public string GetConversantName()
        {
            return conversantName;
        }

        public void Interaction(GameObject interactor)
        {
            if(interactor.GetComponent<NPCStateMachine>() != null) return;
            
            if(dialogues.Count == 0) return;
            PlayerConversant conversant = interactor.GetComponent<PlayerConversant>();
            Dialogue dialogueToDisplay = null;
            foreach(Dialogue dialogue in dialogues)
            {
                if(!dialogue.GetRootNode().CheckCondition(conversant.GetEvaluators())) continue;

                dialogueToDisplay = dialogue;
                break;
            }

            if(dialogueToDisplay == null) return;

            conversant.StartDialogue(this, dialogueToDisplay);
            PlayerStateMachine stateMachine = interactor.GetComponent<PlayerStateMachine>();
            stateMachine.SwitchState(new PlayerDialogueState(stateMachine));    
        }

        public void HandleRaycast(GameObject player)
        {
            TMP_Text interactionText = player.GetComponent<PlayerStateMachine>().InteractionText;
            interactionText.enabled = true;
            interactionText.text = $"Press F to speak with {conversantName}";
            return;  
        }
    }
}

