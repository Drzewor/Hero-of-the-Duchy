using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace RPG.Combat
{
    public class Targeter : MonoBehaviour
    {
        [SerializeField] private CinemachineTargetGroup cineTargetGroup;
        private Camera mainCamera;
        private List<Target> targets = new List<Target>();

        public Target CurrentTarget {get; private set;}

        private void Start() 
        {
            mainCamera = Camera.main;
        }

        private void OnTriggerEnter(Collider other) 
        {
            if(!other.TryGetComponent<Target>(out Target target)) return;
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
            cineTargetGroup.AddMember(CurrentTarget.transform, 1f, 2f);

            return true;
        }

        public void Cancel()
        {
            if(CurrentTarget == null) return;

            cineTargetGroup.RemoveMember(CurrentTarget.transform);
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
    }

}
