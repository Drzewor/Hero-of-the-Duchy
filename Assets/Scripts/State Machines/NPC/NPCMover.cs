using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCMover : MonoBehaviour
    {
        [SerializeField] private Transform pointToGo;
        private NPCStateMachine stateMachine;

        private void Start() 
        {
            stateMachine = GetComponent<NPCStateMachine>();
        }
        //All functions are called from QuestEventActivator
        public void SendToPointAndBack()
        {
            stateMachine.SwitchState(new NPCMovingState(stateMachine,pointToGo.position));
        }

        public void SendToPointAndStay()
        {
            stateMachine.SetHomePosition(pointToGo.position);
        }

        public void FollowTarget(Transform target)
        {
            stateMachine.SetTargetToFollow(target);
            stateMachine.SwitchState(new NPCFollowingState(stateMachine));
        }
    }
}
