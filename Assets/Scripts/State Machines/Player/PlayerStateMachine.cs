using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cinemachine;
using RPG.Combat;
using RPG.Quests;
using RPG.Saving;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace RPG.StateMachine.Player
{
    public class PlayerStateMachine : StateMachine, ISaveable
    {
        private bool isStartingStateSet = false;
        [field: SerializeField] public InputReader InputReader {get; private set;}
        [field: SerializeField] public CharacterController characterController {get; private set;}
        [field: SerializeField] public CinemachineFreeLook cinemachineFreeLook {get; private set;}
        [field: SerializeField] public Animator Animator {get; private set;}
        [field: SerializeField] public Targeter Targeter {get; private set;}
        [field: SerializeField] public Health Health {get; private set;}
        [field: SerializeField] public ForceReceiver ForceReceiver {get; private set;}
        [field: SerializeField] public WeaponLogic WeaponLogic {get; private set;}
        [field: SerializeField] public LedgeDetector LedgeDetector {get; private set;}
        [field: SerializeField] public InventoryManager InventoryManager {get; private set;}
        [field: SerializeField] public Character Character {get; private set;}
        [field: SerializeField] public Stamina Stamina {get; private set;}
        [field: SerializeField] public Mana Mana {get; private set;}
        [field: SerializeField] public QuestManager QuestManager {get; private set;}
        [field: SerializeField] public CharacterExperience CharacterExperience {get; private set;}
        [field: SerializeField] public GameObject PauseWindow {get; private set;}
        [field: SerializeField] public TMP_Text InteractionText {get; private set;}
        [field: SerializeField] public float FreeLookMovmentSpeed {get; private set;}
        [field: SerializeField] public float TargetingMovmentSpeed {get; private set;}
        [field: SerializeField] public float SprintMovmentSpeed {get; private set;}
        [field: SerializeField] public float RotationDamping {get; private set;}
        [field: SerializeField] public float DodgeDuration {get; private set;}
        [field: SerializeField] public float DodgeDistance {get; private set;}
        [field: SerializeField] public float PreviousDodgeTime {get; private set;}  = Mathf.NegativeInfinity;
        [field: SerializeField] public float JumpForce {get; private set;}
        [field: SerializeField] public float AttackDamage {get; private set;}
        [field: SerializeField] public bool IsWeaponMagic {get; private set;}
        [field: SerializeField] public statDamageBonus StatDamageBonus {get; private set;}
        public float DamageBonus 
        {
            get
            {
                return GetDamageBonus(StatDamageBonus);
            }
        }
        [field: SerializeField] public float InteractionRadius = 2f;
        [field: SerializeField] public float InteractionDistance = 10f;
        [field: SerializeField] public Attack[] Attacks {get; private set;}
        public Transform MainCameraTransform {get; private set;}
        [Space(15)] 
        [SerializeField]private bool useStartingState  = true;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            MainCameraTransform = Camera.main.transform;
            StartCoroutine(SetInventory());
            if(useStartingState)
            {
                SetStartingState(new PlayerFreeLookState(this));
            }
        }

        private void OnEnable() 
        {
            Health.OnTakeDamage += HandleTakeDamage;
            Health.OnDie += HandleDeath;
            InputReader.PressESCEvent += SwitchPauseMenu;
        }

        private void OnDisable() 
        {
            Health.OnTakeDamage -= HandleTakeDamage;
            Health.OnDie -= HandleDeath;
            InputReader.PressESCEvent -= SwitchPauseMenu;
        }

        private void HandleTakeDamage()
        {
            SwitchState(new PlayerImpactState(this));
        }

        private void HandleDeath(Health health)
        {
            SwitchState(new PlayerDeadState(this));
        }

        public void SetStartingState(PlayerBaseState state)
        {
            if(!isStartingStateSet)
            {
                SwitchState(state);
                isStartingStateSet = true;
            }
        }

        public void SwitchPauseMenu()
        {
            if(InputReader.ESCHasMultipleHandleres()) return;
            InputReader.isGamePaused = !InputReader.isGamePaused;
            if(InputReader.isGamePaused)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;              
            }
               
            PauseWindow.SetActive(!PauseWindow.activeInHierarchy);
        }

        public void SetDodgeTime(float time)
        {
            PreviousDodgeTime = time;
        }

        public void SetWeaponLogic(WeaponLogic weapon)
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

        public void SetStatDamageBonus(statDamageBonus statDamageBonus)
        {
            StatDamageBonus = statDamageBonus;
        }

        private float GetDamageBonus(statDamageBonus statDamageBonus)
        {
            if(statDamageBonus == statDamageBonus.Strength)
            {
                return Character.Strength.Value;
            }
            else if(statDamageBonus == statDamageBonus.Dexterity)
            {
                return Character.Dexterity.Value;
            }
            else if(statDamageBonus == statDamageBonus.Charisma)
            {
                return Character.Charisma.Value;
            }
            else
            {
                return 0;
            }
        }



        private IEnumerator SetInventory()
        {
            yield return new WaitForEndOfFrame();
            InventoryManager.inventory.gameObject.SetActive(false);
            InventoryManager.EquipmentStats.gameObject.SetActive(false);
        }

        public object CaptureState()
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            data["position"] = new SerializableVector3(transform.position);
            data["rotation"] = new SerializableVector3(transform.eulerAngles);
            return data;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> data = (Dictionary<string, object>)state;
            characterController.enabled = false;
            transform.position = ((SerializableVector3)data["position"]).ToVector();
            transform.eulerAngles = ((SerializableVector3)data["rotation"]).ToVector();
            characterController.enabled = true;
        }
    }

}
