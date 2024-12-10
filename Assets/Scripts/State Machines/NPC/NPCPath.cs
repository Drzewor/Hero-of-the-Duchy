using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.StateMachine.NPC
{
    public class NPCPath : MonoBehaviour
    {
        [SerializeField] private List<Transform> pathPoints;
        private List<Follower> followers = new List<Follower>();
        private const float distanceBuffor = 2;

        private void Update() 
        {
            if(followers.Count == 0) return;

            for(int i = followers.Count-1; i >= 0; i--)
            {
                if(followers[i].PointReached(pathPoints))
                {
                    if(followers[i].pathPointsIndex < pathPoints.Count-1)
                    {
                        followers[i].pathPointsIndex++;
                    }
                    else
                    {
                        RemoveFollower(followers[i]);
                        continue;
                    }

                    followers[i].npcMover.SendToPointAndStay(pathPoints[followers[i].pathPointsIndex]);
                }
            }
        }

        public void AddFollower(NPCMover follower, int pointToGo = 0, bool isPatroling = false)
        {
            followers.Add(new Follower(follower,pointToGo,isPatroling));
            Follower newFollower = followers[followers.Count-1];
            newFollower.npcMover.SendToPointAndStay(pathPoints[newFollower.pathPointsIndex]);
        }

        public void RemoveFollower(Follower follower)
        {
            followers.Remove(follower);
        }

        public void GoToNextPoint()
        {

        }

        public class Follower
        {
            public NPCMover npcMover;
            public int pathPointsIndex;
            public bool isOnPatrol;

            public Follower(NPCMover mover, int pointToGo = 0, bool isPatroling = false)
            {
                npcMover = mover;
                pathPointsIndex = pointToGo;
                isOnPatrol = isPatroling;
            }

            public bool PointReached(List<Transform> pathPoints)
            {
                if(Vector3.Distance(pathPoints[pathPointsIndex].position, npcMover.transform.position) <= distanceBuffor)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

    }
}

