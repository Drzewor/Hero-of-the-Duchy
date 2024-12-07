using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class QuestEventActivator : MonoBehaviour
{
    [SerializeField] private List<QuestTrigger> questTriggers;

    public void CheckForTrigger(string ID, TriggerType triggerType)
    {
        foreach(QuestTrigger trigger in questTriggers)
        {
            if(trigger.triggerType != triggerType) continue;

            if(trigger.ID != ID) continue;
            
            trigger.events.Invoke();
            return;
        }
    }

    [Serializable]
    private class QuestTrigger
    {
        public TriggerType triggerType;
        public string ID;
        public UnityEvent events;
    }

    public enum TriggerType
    {
        QuestStart,
        QuestComplete,
        StepStart,
        StepComplete
    }
}
