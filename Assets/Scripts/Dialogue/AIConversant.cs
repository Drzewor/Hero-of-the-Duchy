using System.Collections;
using System.Collections.Generic;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IInteractable
    {
        [SerializeField] private string conversantName = "NPC Name";
        [SerializeField] private Dialogue dialogue = null;

        public string GetConversantName()
        {
            return conversantName;
        }

        public void Iteraction(GameObject player)
        {
            if(dialogue == null) return;
            PlayerConversant conversant = player.GetComponent<PlayerConversant>();
            conversant.StartDialogue(this, dialogue);
            PlayerStateMachine stateMachine = player.GetComponent<PlayerStateMachine>();
            stateMachine.SwitchState(new PlayerDialogueState(stateMachine));    
        }
    }
}

