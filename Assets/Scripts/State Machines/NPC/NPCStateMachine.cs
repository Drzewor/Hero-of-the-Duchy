using System;
using System.Collections;
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
        private bool isStartingStateSet = false;
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
        [Space(15)] 
        [SerializeField]private bool useStartingState  = true;
        private void Start() 
        {
            Agent.updatePosition = false;
            Agent.updateRotation = false;
            if(Health.isDead) return;
            if(useStartingState)
            {
                SetStartingState(new NPCIdleState(this));
            }
            SetHomePosition(gameObject.transform.position);
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
        
        public void SetStartingState(NPCBaseState state)
        {
            if(!isStartingStateSet)
            {
                SwitchState(state);
                isStartingStateSet = true;
            }
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

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            data["homePosition"] = new SerializableVector3(HomePosition);

            if(TargetToFollow != null && TargetToFollow.TryGetComponent<SaveableEntity>(out SaveableEntity saveableEntity))
            {
                data["targetToFollow"] = saveableEntity.GetUniqueIdentifier();
            }
            else
            {
                data["targetToFollow"] = null;
            }

            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            Controller.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            SetHomePosition(((SerializableVector3)data["homePosition"]).ToVector());

            if(data.ContainsKey("targetToFollow") && data["targetToFollow"] != null)
            {
                string targetID = (string)data["targetToFollow"];
                SaveableEntity[] allEntities = FindObjectsOfType<SaveableEntity>();

                foreach(SaveableEntity entity in allEntities)
                {
                    if(entity.GetUniqueIdentifier() == targetID)
                    {
                        SetTargetToFollow(entity.transform);
                        break;
                    }
                }
            }

            Controller.enabled = true;
            NPCTargeter.currentTarget = null;
        }
    }

}
