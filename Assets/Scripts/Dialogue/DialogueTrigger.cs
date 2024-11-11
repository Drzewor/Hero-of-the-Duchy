using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{
    
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private Trigger[] triggers;
        public IEnumerable<Trigger> Triggers => triggers;


        [Serializable]
        public class Trigger
        {
            [SerializeField] string action;
            [SerializeField] UnityEvent onTrigger;
            public void TriggerAction(string actionToTrigger)
            {
                if(actionToTrigger == action)
                {
                    onTrigger.Invoke();
                }
            }
        }

    }
}
