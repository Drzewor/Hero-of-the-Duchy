using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class NPCTargeter : MonoBehaviour
    {
        public Health target = null;
        public Vector3 lastTargetPosition;
        private void OnTriggerEnter(Collider other) 
        {
            if(other.tag != "Player") return;
            if(other.GetComponent<Health>().isDead) return;
            target = other.gameObject.GetComponent<Health>();
        }

        private void OnTriggerExit(Collider other) 
        {
            if(other.tag != "Player") return;
            lastTargetPosition = target.transform.position;
            target = null;
        }

        public float GetDistanceToTargetSqr()
        {
            if(target == null) return Mathf.Infinity;
            return (target.transform.position - transform.position).sqrMagnitude;
        }

        public void SetLastKnowPosition(Vector3 lastKnowPosition)
        {
            lastTargetPosition = lastKnowPosition;
        }
    }
}

