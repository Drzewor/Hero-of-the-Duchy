using System.Collections;
using System.Collections.Generic;
using RPG.Factions;
using UnityEngine;

namespace RPG.Combat
{
    public class NPCTargeter : MonoBehaviour
    {
        [SerializeField] private FactionManager myFManager;
        [SerializeField] private List<Health> targetsList = new List<Health>();
        public Health currentTarget = null;
        public Vector3 lastTargetPosition;

        private void OnTriggerEnter(Collider other) 
        {
            if(!other.TryGetComponent(out FactionManager fManager)) return;

            if(!myFManager.isEnemy(fManager.GetFaction())) return;

            if(!other.TryGetComponent(out Health health)) return;

            if(health.isDead) return;

            if(currentTarget == null)
            {
                currentTarget = health;
                health.OnDie += RemoveTarget;
                targetsList.Add(health);
            }
            else
            {
                health.OnDie += RemoveTarget;
                targetsList.Add(health);
            }
        }

        private void OnTriggerExit(Collider other) 
        {
            if(!other.TryGetComponent(out Health health)) return;

            RemoveTarget(health);
        }

        private Health GetClosestTarget()
        {
            float closestDistance = Mathf.Infinity;
            float nextTargetDistance;
            Health closestTarget = null;

            foreach (Health target in targetsList)
            {
                nextTargetDistance = GetDistanceToTargetSqr(target.transform);
                if(nextTargetDistance < closestDistance)
                {
                    closestTarget = target;
                }
            }

            return closestTarget;
        }

        private void RemoveTarget(Health health)
        {
            if(health == currentTarget)
            {
                lastTargetPosition = currentTarget.transform.position;
                targetsList.Remove(currentTarget);
                health.OnDie -= RemoveTarget;

                if(targetsList.Count != 0)
                {
                    currentTarget = GetClosestTarget();
                }
                else
                {
                    currentTarget = null;
                }

                return;
            }

            targetsList.Remove(health);
            health.OnDie -= RemoveTarget;
        }

        public float GetDistanceToTargetSqr()
        {
            if(currentTarget == null) return Mathf.Infinity;
            return (currentTarget.transform.position - transform.position).sqrMagnitude;
        }

        public float GetDistanceToTargetSqr(Transform target)
        {
            if(target == null) return Mathf.Infinity;
            return (target.position - transform.position).sqrMagnitude;
        }

        public void SetLastKnowPosition(Vector3 lastKnowPosition)
        {
            lastTargetPosition = lastKnowPosition;
        }
    }
}

