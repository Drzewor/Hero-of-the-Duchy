using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.StateMachine.Player;
using TMPro;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string conversantName = "Player Name";
        [SerializeField] Dialogue testDialogue;
        private AIConversant currentConversant = null;
        private Dialogue currentDialogue;
        private DialogueNode currentNode = null;
        private bool isChoosing = false;
        public event Action onConversationUpdate;

        public void StartDialogue(AIConversant newConversant,Dialogue newDialogue)
        {
            currentConversant = newConversant;
            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();
            TriggerEnterAction();

            onConversationUpdate();
        }

        public void Quit()
        {
            currentDialogue = null;
            TriggerExitAction();
            currentConversant = null;
            currentNode = null;
            isChoosing = false;

            onConversationUpdate();
            PlayerStateMachine stateMachine = gameObject.GetComponent<PlayerStateMachine>();
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }

        public bool IsActive()
        {
            return currentDialogue != null;
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public string GetText()
        {
            if (currentNode == null) return "";
            
            return currentNode.GetText();
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            currentNode = chosenNode;
            TriggerEnterAction();
            isChoosing = false;
            Next();
        }

        public void Next()
        {
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();

                onConversationUpdate();
                return;
            }
            isChoosing = false;

            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();

            onConversationUpdate();
        }

        public bool HasNext()
        {
            
            return currentDialogue.GetAllChildren(currentNode).Count() > 0;
        }

        private void TriggerEnterAction()
        {
            if(currentNode !=null)
            {
                TriggerAction(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if(currentNode !=null)
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if(action == "") return;

            foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
            {
                trigger.Trigger(action);
            }
        }

        public string GetCurrentConversantName()
        {
            if(isChoosing)
            {
                return conversantName;
            }
            else
            {
                return currentConversant.GetConversantName();
            }
        }
    }

}