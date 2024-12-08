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

        public void SwapTarget()
        {
            if(isChangingTarget) return;

            Target newTarget = null;
            int currentTargetIndex = targets.IndexOf(CurrentTarget);
            int i;

            if(currentTargetIndex == targets.Count - 1)
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

            if(newTarget == null) return;
            activeCorutine = StartCoroutine(SwitchTargets(CurrentTarget,newTarget));
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
            if(CurrentTarget == target)
            {
                cineTargetGroup.RemoveMember(CurrentTarget.transform);
                CurrentTarget = null;
            }

            target.OnDisabled -= RemoveTarget;
            targets.Remove(target);
        }

        private IEnumerator SwitchTargets(Target oldTarget, Target newTarget)
        {
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
        }
    }

}
