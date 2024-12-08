using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCMover : MonoBehaviour
    {
        [SerializeField] Transform pointToGo;
        [SerializeField] NPCStateMachine stateMachine;
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
            stateMachine.SwitchState(new NPCFollowingState(stateMachine, target));
        }
    }
}
