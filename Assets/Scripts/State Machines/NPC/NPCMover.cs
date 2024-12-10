using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCMover : MonoBehaviour
    {
        private NPCStateMachine stateMachine;

        private void Start() 
        {
            stateMachine = GetComponent<NPCStateMachine>();
        }
        //All functions are called from QuestEventActivator
        public void SendToPointAndBack(Transform pointToGo)
        {
            stateMachine.SetTargetToFollow(null);
            stateMachine.SwitchState(new NPCMovingState(stateMachine,pointToGo.position));
        }

        public void SendToPointAndStay(Transform pointToGo)
        {
            stateMachine.SetTargetToFollow(null);
            stateMachine.SetHomePosition(pointToGo.position);
            stateMachine.SwitchState(new NPCMovingState(stateMachine,pointToGo.position));
        }

        public void FollowTarget(Transform target)
        {
            stateMachine.SetTargetToFollow(target);
            stateMachine.SwitchState(new NPCFollowingState(stateMachine));
        }

        public void FollowPath(NPCPath npcPath)
        {
            npcPath.AddFollower(this);
        }
    }
}
