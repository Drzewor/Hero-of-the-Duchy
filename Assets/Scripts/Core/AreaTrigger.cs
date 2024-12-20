using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Core
{
    public class AreaTrigger : MonoBehaviour, ISaveable
    {
        [SerializeField] List<UnityEvent> onTriggers;
        private bool wasActivated = false;

        private void OnTriggerEnter(Collider other) 
        {
            if(!wasActivated)
            {
                if(other.tag != "Player") return;
            
                if(onTriggers.Count == 0) return;

                foreach(UnityEvent unityEvent in onTriggers)
                {
                    unityEvent.Invoke();
                }
                wasActivated = true;
            }
        }

        public object CaptureState()
        {
            return wasActivated;
        }

        public void RestoreState(object state)
        {
            if(state == null) return;
            wasActivated = (bool)state;
        }
    }

}
