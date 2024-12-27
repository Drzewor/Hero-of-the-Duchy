using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using RPG.Factions;
using UnityEngine;

namespace RPG.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;
        [SerializeField] private FactionManager factionManager;
        [SerializeField] private float targetSwitchSpeed = 1f;
        private Camera mainCamera;
        private const float targetWeight = 1f;
        private const float targetRadius = 1f;
        private bool isChangingTarget = false;
        private Coroutine activeCorutine = null;
        private Target oldTargetInChange = null;
        private List<Target> targets = new List<Target>();

        public Target CurrentTarget {get; private set;}

        private void Start() 
        {
            mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(!other.TryGetComponent<Target>(out Target target)) return;
            if(other.TryGetComponent<FactionManager>(out FactionManager manager)) 
            {
                if(factionManager.isAlly(manager.GetFaction())) return;
            }
            targets.Add(target);
            target.OnDisabled += RemoveTarget;
        }

        private void OnTriggerExit(Collider other) 
        {
            if(!other.TryGetComponent<Target>(out Target target)) return;
            
            RemoveTarget(target);
        }

        public bool SelectTarget()
        {
            if(targets.Count == 0) return false;
            Target closestTarget = null;
            float closestTargetDistance = Mathf.Infinity;

            foreach(Target target in targets)
            {
                Vector2 viewPosition = mainCamera.WorldToViewportPoint(target.transform.position);
                if(!target.GetComponentInChildren<Renderer>().isVisible) continue;

                Vector2 toCenter = viewPosition - new Vector2(0.5f,0.5f);
                if(toCenter.sqrMagnitude < closestTargetDistance)
                {
                    closestTarget = target;
                    closestTargetDistance = toCenter.sqrMagnitude;
                }
            }
            if(closestTarget == null) return false;

            CurrentTarget = closestTarget;
            cineTargetGroup.AddMember(CurrentTarget.transform, targetWeight, targetRadius);

            return true;
        }

        public void SwapTarget(bool isCurrentTargetGone)
        {
            if(isChangingTarget) return;

            Target newTarget = null;
            int currentTargetIndex = 0;
            int i;

            if(CurrentTarget != null)
            {
                currentTargetIndex = targets.IndexOf(CurrentTarget);
            }

            if(currentTargetIndex == targets.Count - 1 || currentTargetIndex == -1)
            {
                i = 0;
            }
            else
            {
                i = currentTargetIndex;
            }

            for(; i < targets.Count; i++)
            {
                if(targets[i] == CurrentTarget) continue;
                if(!targets[i].GetComponentInChildren<Renderer>().isVisible) continue;
                newTarget = targets[i];
                break;
            }

            if(newTarget == null && isCurrentTargetGone)
            {
                cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
                return;
            }

            if(newTarget == null) return;
            activeCorutine = StartCoroutine(SwitchTargets(CurrentTarget,newTarget,isCurrentTargetGone));
        }

        public void Cancel()
        {
            if(CurrentTarget == null) return;
            if(activeCorutine != null)
            {
                StopCoroutine(activeCorutine);
                isChangingTarget = false;
            }
            
            for(int i = cineTargetGroup.m_Targets.Count() - 1; i > 0; i--)
            {
                cineTargetGroup.RemoveMember(cineTargetGroup.m_Targets[i].target);
            }
            CurrentTarget = null;
            
        }

        private void RemoveTarget(Target target)
        {
            target.OnDisabled -= RemoveTarget;

            if(CurrentTarget == target)
            {
                SwapTarget(true);
            }

            if(oldTargetInChange != target)
            {
                targets.Remove(target);
            }
        }

        private IEnumerator SwitchTargets(Target oldTarget, Target newTarget, bool isOldTargetGone)
        {
            oldTargetInChange = oldTarget;
            isChangingTarget = true;
            cineTargetGroup.AddMember(newTarget.transform, 0, targetRadius);
            int oldTargetIndex = cineTargetGroup.FindMember(oldTarget.transform);
            int newTargetIndex = cineTargetGroup.FindMember(newTarget.transform);
            CurrentTarget = newTarget;

            while(cineTargetGroup.m_Targets[newTargetIndex].weight < targetWeight || cineTargetGroup.m_Targets[oldTargetIndex].weight >= 0)
            {
                if(cineTargetGroup.m_Targets[newTargetIndex].weight < targetWeight)
                {
                    cineTargetGroup.m_Targets[newTargetIndex].weight += targetSwitchSpeed * Time.deltaTime;
                }
                if(cineTargetGroup.m_Targets[oldTargetIndex].weight >= 0)
                {

                    cineTargetGroup.m_Targets[oldTargetIndex].weight -= targetSwitchSpeed * Time.deltaTime;
                }
                yield return null;
            }

            cineTargetGroup.RemoveMember(oldTarget.transform);
            isChangingTarget = false;
            oldTargetInChange = null;

            if(isOldTargetGone || GetComponent<SphereCollider>().radius < 
            Vector3.Distance(oldTarget.transform.position,transform.position))
            {
                targets.Remove(oldTarget);
            }

            if(CurrentTarget.GetComponent<Health>().isDead || GetComponent<SphereCollider>().radius < 
            Vector3.Distance(CurrentTarget.transform.position,transform.position))
            {
                SwapTarget(true);
            }
        }
    }

}
