using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Saving;
using RPG.Combat;
using RPG.CharacterStats;
using RPG.Inventories;

namespace RPG.StateMachine.NPC
{
    public class NPCStateMachine : StateMachine, ISaveable
    {
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public NPCTargeter NPCTargeter {get; private set;}
        [field: SerializeField] public CharacterController Controller {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public NavMeshAgent Agent {get; private set;}
        [field: SerializeField] public WeaponLogic WeaponLogic {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public Target Target {get; private set;}
        [field: SerializeField] public Character Character {get; private set;}
        [field: SerializeField] public float MovementSpeed {get; private set;}
        [field: SerializeField] public float RunningSpeed {get; private set;} = 5f;
        [field: SerializeField] public float AttackRange {get; private set;}
        [field: SerializeField] public float AttackDamage {get; private set;} = 10;
        [field: SerializeField] public float StatDamageBonus {get; private set;} = 0;
        [field: SerializeField] public bool IsWeaponMagic {get; private set;} = false;
        [field: SerializeField] public float AttackKnockback {get; private set;} = 0;  
        [field: SerializeField] public float MaxCircleRange {get; private set;} = 10;  
        [field: SerializeField] public float MinCircleRange {get; private set;} = 5.5f;  
        [field: SerializeField] public float MaxCirclingTime {get; private set;} = 4;  
        [field: SerializeField] public float MinCirclingTime {get; private set;} = 0.5f;
        [field: SerializeField] public float CirclingSpeed {get; private set;} = 3.5f;
        [field: SerializeField] public float ChargeRange {get; private set;} = 5f;
        [field: SerializeField] public float DodgeDuration {get; private set;} = 1;
        [field: SerializeField] public float DodgeDistance {get; private set;} = 4;
        [field: SerializeField] public Vector3 HomePosition {get; private set;}
        [field: SerializeField] public float MaxSuspiciousTime {get; private set;} = 2;
        [field: SerializeField] public float MinSuspiciousTime {get; private set;} = 6;
        [field: SerializeField] public Transform TargetToFollow {get; private set;}
        [field: SerializeField] public Attack[] Attacks {get; private set;}    
        [field: SerializeField] public GameObject ObjectToInteract {get; private set;} = null;
        private void Start() 
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            if(Health.isDead) return;
            SetHomePosition(gameObject.transform.position);
            if(ObjectToInteract != null && ObjectToInteract.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                SwitchState(new NPCInteractState(this, interactable, ObjectToInteract.transform.position));
                return;
            }
            SwitchState(new NPCIdleState(this));
            
        }

        private void OnEnable() 
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
        }

        private void OnDisable() 
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new NPCImpactState(this));
        }
        
        private void HandleDeath(Health health)
        {
            SwitchState(new NPCDeadState(this));
        }

        public void SetWeapon(WeaponLogic weapon)
        {
            this.WeaponLogic = weapon;
        }

        public void SetAttacks(Attack[] attacks)
        {
            Attacks = attacks;
        }

        public void SetAttackDamage(float attackDamage)
        {
            AttackDamage = attackDamage;
        }

        public void SetIsWeaponMagic(bool isMagic)
        {
            IsWeaponMagic = isMagic;
        }

        public void SetAttackRange(float attackRange)
        {
            AttackRange = attackRange;
        }

        public void SetHomePosition(Vector3 HomePoint)
        {
            HomePosition = HomePoint;
        }

        public void SetTargetToFollow(Transform target)
        {
            TargetToFollow = target;
        }

        public void SetMovementSpeed(float speed)
        {
            MovementSpeed = speed;
        }

        public void SetObjectToInteract(GameObject objectToInteract)
        {
            ObjectToInteract = objectToInteract;
        }

        public void SetStatDamageBonus(statDamageBonus statDamageBonus)
        {
            if(statDamageBonus == statDamageBonus.Strength)
            {
                StatDamageBonus = Character.Strength.Value;
                return;
            }
            else if(statDamageBonus == statDamageBonus.Dexterity)
            {
                StatDamageBonus = Character.Dexterity.Value;
                return;
            }
            else if(statDamageBonus == statDamageBonus.Charisma)
            {
                StatDamageBonus = Character.Charisma.Value;
                return;
            }
            else
            {
                StatDamageBonus = 0;
            }
        }

        //for the future use
        public void Interact(GameObject objectToInteract)
        {
            if(objectToInteract.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                SwitchState(new NPCInteractState(this,interactable,objectToInteract.transform.position));
            }
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            data["homePosition"] = new SerializableVector3(HomePosition);

            if(TargetToFollow != null && TargetToFollow.TryGetComponent<SaveableEntity>(out SaveableEntity targetToFollow))
            {
                data["targetToFollow"] = targetToFollow.GetUniqueIdentifier();
            }
            else
            {
                data["targetToFollow"] = null;
            }

            if(ObjectToInteract != null && ObjectToInteract.TryGetComponent<SaveableEntity>(out SaveableEntity objectToInteract))
            {
                data["objectToInteract"] = objectToInteract.GetUniqueIdentifier();
            }
            else
            {
                data["objectToInteract"] = null;
            }

            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            bool lookForTargetToFollow = false;
            string TargetToFollowID = null;
            bool lookForObjectToInteract = false;
            string ObjectToInteractID = null;

            Controller.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            SetHomePosition(((SerializableVector3)data["homePosition"]).ToVector());

            if(data.ContainsKey("targetToFollow"))
            {
                if(data["targetToFollow"] == null)
                {
                    SetTargetToFollow(null);
                }
                else
                {
                    lookForTargetToFollow = true;
                    TargetToFollowID = (string)data["targetToFollow"];
                }
            }
            if(data.ContainsKey("objectToInteract"))
            {
                if(data["objectToInteract"] == null)
                {
                    SetObjectToInteract(null);
                }
                else
                {
                    lookForObjectToInteract = true;
                    ObjectToInteractID = (string)data["objectToInteract"];
                }
            }

            if(lookForTargetToFollow || lookForObjectToInteract)
            {
                SaveableEntity[] allEntities = FindObjectsOfType<SaveableEntity>();
                foreach(SaveableEntity entity in allEntities)
                {
                    if(lookForTargetToFollow && entity.GetUniqueIdentifier() == TargetToFollowID)
                    {
                        SetTargetToFollow(entity.transform);
                        lookForTargetToFollow = false;
                    }
                    if(lookForObjectToInteract && entity.GetUniqueIdentifier() == ObjectToInteractID)
                    {
                        SetObjectToInteract(entity.gameObject);
                        lookForObjectToInteract = false;
                    }

                    if(!lookForTargetToFollow && !lookForObjectToInteract)
                    {
                        break;
                    }
                }
            }


            Controller.enabled = true;
            NPCTargeter.currentTarget = null;
        }
    }

}
