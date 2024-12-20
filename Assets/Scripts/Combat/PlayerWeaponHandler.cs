using System.Collections;
using System.Collections.Generic;
using RPG.Inventories;
using RPG.Saving;
using RPG.StateMachine.Player;
using UnityEngine;

namespace RPG.Combat
{
    public class PlayerWeaponHandler : MonoBehaviour
    {
        [SerializeField] PlayerStateMachine stateMachine;
        [SerializeField] public WeaponItem DefaultWeapon;
        [SerializeField] private Transform weaponSlot;
        private WeaponItem equippedWeapon;
        private WeaponLogic weaponLogic;
        private GameObject weaponInstance;

        private void Awake() 
        {
            EquipWeapon(DefaultWeapon);
        }

        public void EquipWeapon(WeaponItem weapon)
        {
            if(weapon == null)
            {
                weapon = DefaultWeapon;
            }
            this.equippedWeapon = weapon;
            if(weaponInstance != null)
            {
                Destroy(weaponInstance);
            }
            weaponInstance = Instantiate(weapon.weaponPrefab, weaponSlot).gameObject;
            weaponLogic = weaponInstance.GetComponentInChildren<WeaponLogic>();
            weaponLogic.SetMyCollider(stateMachine.GetComponent<Collider>());
            weaponLogic.OnKill += HandleKill;
            SetWeapon();
            DisableWepon();
        }

        //Called in Animation
        public void EnableWepon()
        {
            weaponLogic.gameObject.GetComponent<Collider>().enabled = true;
            weaponLogic.SwitchTrails(true);

        }

        //Called in Animation
        public void DisableWepon()
        {
            weaponLogic.gameObject.GetComponent<Collider>().enabled = false;
            weaponLogic.ClearColidersList();
            weaponLogic.SwitchTrails(false);

        }

        //Called in Animation
        public void Shoot()
        {
            if(weaponLogic.projectilePrefab == null) return;
            if(stateMachine.Targeter.CurrentTarget ==null)
            {
                weaponLogic.LaunchProjectile(stateMachine.gameObject);
                return;
            }
            weaponLogic.LaunchProjectile(stateMachine.gameObject, stateMachine.Targeter.CurrentTarget.gameObject);
        }

        private void SetWeapon()
        {
            stateMachine.SetWeaponLogic(weaponLogic);

            stateMachine.SetAttackDamage(equippedWeapon.weaponDamage);

            stateMachine.SetStatDamageBonus(equippedWeapon.statDamageBonus);

            stateMachine.SetAttacks(equippedWeapon.attacks);

            stateMachine.SetIsWeaponMagic(equippedWeapon.isMagical);
        }

        public void HandleKill(GameObject victim)
        {
            stateMachine.QuestManager.TryToAdvanceQuests(victim.name);

            NPCLoot victimLoot = victim.GetComponent<NPCLoot>();
            if(victimLoot == null) return;

            victimLoot.GrantExpBounty(stateMachine.CharacterExperience);
            victimLoot.SpawnLoot();
        }
    }
}
