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
            return FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode));
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
            int numPlayerResponses = FilterOnCondition(currentDialogue.GetPlayerChildren(currentNode)).Count();
            if(numPlayerResponses > 0)
            {
                isChoosing = true;
                TriggerExitAction();

                onConversationUpdate();
                return;
            }
            isChoosing = false;

            DialogueNode[] children = FilterOnCondition(currentDialogue.GetAIChildren(currentNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, children.Count());
            TriggerExitAction();
            currentNode = children[randomIndex];
            TriggerEnterAction();

            onConversationUpdate();
        }

        public bool HasNext()
        {
            return FilterOnCondition(currentDialogue.GetAllChildren(currentNode)).Count() > 0;
        }

        public IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
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

        private IEnumerable<DialogueNode> FilterOnCondition(IEnumerable<DialogueNode> inputNode)
        {
            foreach(DialogueNode node in inputNode)
            {
                if(node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private void TriggerAction(List<string> actions)
        {
            if(actions.Count == 0) return;
            DialogueTrigger dialogueTrigger;

            if(!currentConversant.TryGetComponent<DialogueTrigger>(out dialogueTrigger)) return;
            
            foreach (string action in actions)
            {
                foreach(var trigger in dialogueTrigger.Triggers)
                {
                    trigger.TriggerAction(action);
                }
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